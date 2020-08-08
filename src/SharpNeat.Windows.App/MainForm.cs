﻿/* ***************************************************************************
 * This file is part of SharpNEAT - Evolution of Neural Networks.
 * 
 * Copyright 2004-2016 Colin Green (sharpneat@gmail.com)
 *
 * SharpNEAT is free software; you can redistribute it and/or modify
 * it under the terms of The MIT License (MIT).
 *
 * You should have received a copy of the MIT License
 * along with SharpNEAT; if not, see https://opensource.org/licenses/MIT.
 */
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace SharpNeat.Windows.App
{
    /// <summary>
    /// SharpNEAT main GUI window.
    /// </summary>
    public partial class MainForm : Form
    {
        private static readonly ILog __log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Form Constructor / Initialisation

        /// <summary>
        /// Construct and initialize the form.
        /// </summary>
        public MainForm()
        {
            // Set the default culture for all threads in the application to the Invariant culture.
            // This is a cheap way of ensuring that all form fields and data IO routines
            // read and write textual data in the same format, in particular the use of a dot as the
            // decimal separator (some culture use a comma).
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            InitializeComponent();
            Logger.SetListBox(lbxLog);

            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.properties"));
        }

        #endregion
    }
}
