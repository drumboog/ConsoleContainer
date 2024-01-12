using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleContainer.Wpf.Domain.Contracts
{
    public interface IProcessInformation
    {
        string? ProcessName { get; }
        string? FileName { get; }
        string? Arguments { get; }
        string? WorkingDirectory { get; }
    }
}
