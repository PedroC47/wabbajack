﻿using Alphaleonis.Win32.Filesystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wabbajack.Common;
using Wabbajack.Lib;
using Wabbajack.Lib.ModListRegistry;

namespace Wabbajack.UI
{
    public class ModeSelectionWindowViewModel : ViewModel
    {


        public ModeSelectionWindowViewModel()
        {
            _modLists = new ObservableCollection<ModlistMetadata>(ModlistMetadata.LoadFromGithub());
        }

        private ObservableCollection<ModlistMetadata> _modLists;

        public ObservableCollection<ModlistMetadata> ModLists
        {
            get => _modLists;
        }


        private ModlistMetadata _selectedModList;
        public ModlistMetadata SelectedModList
        {
            get => _selectedModList;
            set
            {
                CanInstall = true;
                RaiseAndSetIfChanged(ref _selectedModList, value);
            }
        }

        private bool _canInstall;

        public bool CanInstall
        {
            get => _canInstall;
            set => RaiseAndSetIfChanged(ref _canInstall, value);
        }

        internal string Download()
        {
            if (!Directory.Exists(Consts.ModListDownloadFolder))
                Directory.CreateDirectory(Consts.ModListDownloadFolder);

            string dest = Path.Combine(Consts.ModListDownloadFolder, SelectedModList.Links.MachineURL + ExtensionManager.Extension);

            var window = new DownloadWindow(SelectedModList.Links.Download, SelectedModList.Title, dest);
            window.ShowDialog();

            if (window.Result == DownloadWindow.WindowResult.Completed)
                return dest;
            return null;

        }
    }
}