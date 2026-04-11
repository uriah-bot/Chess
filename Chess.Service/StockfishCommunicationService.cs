using System.Diagnostics;

namespace Chess.Service
{
    public class StockfishCommunicationService : IDisposable
    {
        private Process _stockfishProcess = default!;
        private StreamWriter _engineInput = default!;
        private StreamReader _engineOutput = default!;

        public void StartEngine(string pathToExe)
        {
            var proccessStartInfo = new ProcessStartInfo
            {
                FileName = pathToExe,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                CreateNoWindow = true,
            };

            _stockfishProcess = new Process { StartInfo = proccessStartInfo };
            _stockfishProcess.Start();

            _engineInput = _stockfishProcess.StandardInput;
            _engineOutput = _stockfishProcess.StandardOutput;

            _engineInput.WriteLine("uci");
        }

        public void SetEngineOption(string optionName, string optionValue)
        {
            if (_stockfishProcess == null || _stockfishProcess.HasExited)
            {
                return;
            }

            _engineInput.WriteLine($"setoption name {optionName} value {optionValue}");
        }

        public void ConfigureBotDifficulty(int? estimatedElo)
        {
            if (estimatedElo == null || estimatedElo < 1320)
            {
                return;
            }

            // modern Elo limiter (Min is ~1320)
            SetEngineOption("UCI_LimitStrength", "true");
            SetEngineOption("UCI_Elo", $"{estimatedElo}");
        }

        public async Task<string> GetBotMoveAsync(string FEN, int? elo = null)
        {
            if (_stockfishProcess == null || _stockfishProcess.HasExited)
            {
                throw new InvalidOperationException("Stockfish isn't running");
            }

            ConfigureBotDifficulty(elo);

            await _engineInput.WriteLineAsync("isready");
            while (true)
            {
                string readyLine = await _engineOutput.ReadLineAsync();

                if (readyLine == "readyok")
                {
                    break;
                }
            }

            await _engineInput.WriteLineAsync($"position fen {FEN}");

            while (true)
            {
                string line = await _engineOutput.ReadLineAsync();

                if (line != null && line.StartsWith("bestmove"))
                {
                    string[] parts = line.Split(' ');
                    if (parts.Length >= 2)
                    {
                        return parts[1]; // example: bestmove e2e4
                    }
                }
            }
        }
        public void Dispose()
        {
            if ( _stockfishProcess != null && !_stockfishProcess.HasExited )
            {
                _engineInput?.WriteLine("quit");
                _stockfishProcess.WaitForExit(1000);
                _stockfishProcess.Dispose();
            }
        }
    }
}
