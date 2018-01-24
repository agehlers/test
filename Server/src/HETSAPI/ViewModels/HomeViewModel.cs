﻿namespace HETSAPI.ViewModels
{
    /// <summary>
    /// Home Page Model
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// User Id (current user)
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Development Environment
        /// </summary>
        public bool DevelopmentEnvironment { get; set; }

        /// <summary>
        /// Current Context Request Id
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Message (error)
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Inner Exception Message (error)
        /// </summary>
        public string InnerMessage { get; set; }

        /// <summary>
        /// Source (error)
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// OriginalUrl (error)
        /// </summary>
        public string OriginalUrl { get; set; }
    }
}
