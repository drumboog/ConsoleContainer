using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleContainer.Wpf.Eventing
{
    /// <summary>
    /// Access the current <see cref="IPlatformProvider"/>.
    /// </summary>
    public static class PlatformProvider
    {
        /// <summary>
        /// Gets or sets the current <see cref="IPlatformProvider"/>.
        /// </summary>
        public static IPlatformProvider Current { get; set; } = new DefaultPlatformProvider();
    }
}
