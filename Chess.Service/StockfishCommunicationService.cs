using Chess.Model;
using System.Diagnostics;

namespace Chess.Service
{
    public class StockfishCommunicationService : IDisposable
    {
        private Process _stockfishProcess;
        private StreamWriter _engineInput;
        private StreamReader _engineOutput;
        private readonly SemaphoreSlim _stockfishLock = new SemaphoreSlim(1, 1);

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

        private void SetEngineOption(string optionName, string optionValue)
        {
            if (_stockfishProcess == null || _stockfishProcess.HasExited)
            {
                return;
            }

            _engineInput.WriteLine($"setoption name {optionName} value {optionValue}");
        }

        public void ConfigureBotDifficulty(int? estimatedElo)
        {
            if (estimatedElo == null)
            {
                return;
            }

            int safeElo = Math.Clamp(estimatedElo.Value, 1320, 3190);

            // modern Elo limiter (Min is ~1320)
            SetEngineOption("UCI_LimitStrength", "true");
            SetEngineOption("UCI_Elo", $"{safeElo}");

            // if the Elo is below 1320, we can use the old skill level limiter to get a weaker bot (Min is 0)
            if (estimatedElo < 1320)
            {
                SetEngineOption("UCI_LimitStrength", "false");
                SetEngineOption("Skill Level", "1");
            }
        }

        public async Task<string> GetBotMoveAsync(FEN FEN, int? elo = null)
        {
            if (_stockfishProcess == null || _stockfishProcess.HasExited)
            {
                throw new InvalidOperationException("Stockfish isn't running");
            }

            await _stockfishLock.WaitAsync();

            try
            {
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

                await _engineInput.WriteLineAsync($"position fen {FEN.ToString()}");

                // Scale the depth based on the requested ELO
                if (elo <= 1000)
                    await _engineInput.WriteLineAsync("go depth 3"); // shallow, misses simple tactics
                else if (elo <= 1500)
                    await _engineInput.WriteLineAsync("go depth 5"); // club player depth
                else if (elo <= 2000)
                    await _engineInput.WriteLineAsync("go depth 7"); // tournament player depth
                else
                    await _engineInput.WriteLineAsync("go depth 12"); // grandmaster level depth

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
            finally
            {
                _stockfishLock.Release();
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
