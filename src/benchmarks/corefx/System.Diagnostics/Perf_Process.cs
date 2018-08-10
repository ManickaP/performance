﻿using BenchmarkDotNet.Attributes;

namespace System.Diagnostics
{
    public class Perf_Process
    {
        private readonly string _nonExistingName = Guid.NewGuid().ToString();
        private int _currentProcessId;
        
        [Benchmark]
        public void GetCurrentProcess() => Process.GetCurrentProcess().Dispose();
        
        [GlobalSetup(Target = nameof(GetProcessById))]
        public void SetupGetProcessById() => _currentProcessId = Process.GetCurrentProcess().Id;
        
        [Benchmark]
        public void GetProcessById() => Process.GetProcessById(_currentProcessId).Dispose();

        [Benchmark]
        public void EnterLeaveDebugMode()
        {
            Process.EnterDebugMode();
            Process.LeaveDebugMode();
        }
        
        [Benchmark]
        public void GetProcesses()
        {
            foreach (var process in Process.GetProcesses())
            {
                process.Dispose();
            }
        }
        
        [Benchmark]
        public void GetProcessesByName()
        {
            foreach (var process in Process.GetProcessesByName(_nonExistingName))
            {
                process.Dispose();
            }
        }
        
        [Benchmark]
        public void StartAndKillDotnetInfo()
        {
            using (var dotnetInfo = Process.Start(CreateStartInfo()))
            {
                dotnetInfo.Kill();
            }
        }

        private ProcessStartInfo CreateStartInfo()
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = "dotnet",
                Arguments = "--info"
            };

            // this benchmark will run on CI machines where there is no dotnet in PATH
            processStartInfo.EnvironmentVariables["DOTNET_MULTILEVEL_LOOKUP"] = "0"; 

            return processStartInfo;
        }
    }
}