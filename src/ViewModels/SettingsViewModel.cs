﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Disasmo.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Disasmo
{
    public class SettingsViewModel : ViewModelBase
    {
        private string _pathToLocalCoreClr;
        private bool _jitDumpInsteadOfDisasm;
        private string _customEnvVars;
        private bool _showPrologueEpilogue;
        private bool _showAsmComments;
        private bool _skipDotnetRestoreStep;
        private bool _useBdnDisasm;
        private string _bdnRecursionDepth;
        private bool _bdnShowSource;
        private bool _bdnShowIl;
        private bool _bdnShowAsm;

        public SettingsViewModel()
        {
            PathToLocalCoreClr = Settings.Default.PathToCoreCLR;
            JitDumpInsteadOfDisasm = Settings.Default.JitDumpInsteadOfDisasm;
            ShowAsmComments = Settings.Default.ShowAsmComments;
            ShowPrologueEpilogue = Settings.Default.ShowPrologueEpilogue;
            CustomEnvVars = Settings.Default.CustomEnvVars;
            JitDumpInsteadOfDisasm = Settings.Default.JitDumpInsteadOfDisasm;
            SkipDotnetRestoreStep = Settings.Default.SkipDotnetRestoreStep;
            UseBdnDisasm = Settings.Default.UseBdnDisasm;
            BdnShowAsm = Settings.Default.BdnShowAsm;
            BdnShowIL = Settings.Default.BdnShowIL;
            BdnShowSource = Settings.Default.BdnShowSource;
            BdnRecursionDepth = Settings.Default.BdnRecursionDepth;
        }

        public string PathToLocalCoreClr
        {
            get => _pathToLocalCoreClr;
            set
            {
                Set(ref _pathToLocalCoreClr, value);
                Settings.Default.PathToCoreCLR = value;
                Settings.Default.Save();
            }
        }

        public bool JitDumpInsteadOfDisasm
        {
            get => _jitDumpInsteadOfDisasm;
            set
            {
                Set(ref _jitDumpInsteadOfDisasm, value);
                Settings.Default.JitDumpInsteadOfDisasm = value;
                Settings.Default.Save();
            }
        }

        public bool ShowAsmComments
        {
            get => _showAsmComments;
            set
            {
                Set(ref _showAsmComments, value);
                Settings.Default.ShowAsmComments = value;
                Settings.Default.Save();
            }
        }

        public bool ShowPrologueEpilogue
        {
            get => _showPrologueEpilogue;
            set
            {
                Set(ref _showPrologueEpilogue, value);
                Settings.Default.ShowPrologueEpilogue = value;
                Settings.Default.Save();
            }
        }

        public string CustomEnvVars
        {
            get => _customEnvVars;
            set
            {
                Set(ref _customEnvVars, value);
                Settings.Default.CustomEnvVars = value;
                Settings.Default.Save();
            }
        }

        public bool SkipDotnetRestoreStep
        {
            get => _skipDotnetRestoreStep;
            set
            {
                Set(ref _skipDotnetRestoreStep, value);
                Settings.Default.SkipDotnetRestoreStep = value;
                Settings.Default.Save();
            }
        }

        public bool UseBdnDisasm
        {
            get => _useBdnDisasm;
            set
            {
                Set(ref _useBdnDisasm, value);
                Settings.Default.UseBdnDisasm = value;
                Settings.Default.Save();
            }
        }

        public bool BdnShowAsm
        {
            get => _bdnShowAsm;
            set
            {
                Set(ref _bdnShowAsm, value);
                Settings.Default.BdnShowAsm = value;
                Settings.Default.Save();
            }
        }

        public bool BdnShowIL
        {
            get => _bdnShowIl;
            set
            {
                Set(ref _bdnShowIl, value);
                Settings.Default.BdnShowIL = value;
                Settings.Default.Save();
            }
        }

        public bool BdnShowSource
        {
            get => _bdnShowSource;
            set
            {
                Set(ref _bdnShowSource, value);
                Settings.Default.BdnShowSource = value;
                Settings.Default.Save();
            }
        }

        public int BdnRecursionDepthNumeric => byte.TryParse(BdnRecursionDepth, out byte value) ? value : 0;

        public string BdnRecursionDepth
        {
            get => _bdnRecursionDepth;
            set
            {
                if (byte.TryParse(value, out byte result)) // let's limit it with 0-255 range via byte
                {
                    Set(ref _bdnRecursionDepth, value);
                    Settings.Default.BdnRecursionDepth = value;
                    Settings.Default.Save();
                }
                else
                {
                    Set(ref _bdnRecursionDepth, "0");
                }
            }
        }

        public ICommand BrowseCommand => new RelayCommand(() =>
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                PathToLocalCoreClr = dialog.SelectedPath;
        });

        public void FillWithUserVars(Dictionary<string, string> dictionary)
        {
            if (string.IsNullOrWhiteSpace(CustomEnvVars))
                return;

            var pairs = CustomEnvVars.Split(new [] {",", ";"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var pair in pairs)
            {
                var parts = pair.Split('=');
                if (parts.Length == 2)
                    dictionary[parts[0].Trim()] = parts[1].Trim();
            }
        }
    }
}
