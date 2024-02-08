﻿// Source based on https://github.com/Caliburn-Micro/Caliburn.Micro

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
