using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace SteamLauncher.Domain
{
    public class CommandLineSteamProxy : ISteamProxy
    {
        private IProcessProxy _processProxy;
        private string _steamExePath;

        public string BasePath { get; private set; }
        public string UsersPath { get; private set; }
        public string ApplicationsPath { get; private set; }

        public bool IsSilent { get; set; }

        public CommandLineSteamProxy(IProcessProxy processProxy, string steamPath)
        {
            _processProxy = processProxy;
            _steamExePath = steamPath;
            BasePath = Path.GetDirectoryName(_steamExePath);
            UsersPath = Path.Combine(BasePath, "userdata");
            ApplicationsPath = Path.Combine(BasePath, @"Steamapps\Common");
        }

        public void LaunchApp(int id)
        {
            LaunchApp(id, null);
        }

        public void LaunchApp(int id, params string[] applicationParameters)
        {
            var parameters = new List<string>();
            parameters.Add(id.ToString());

            if (applicationParameters != null)
                parameters.AddRange(applicationParameters);

            ExecuteSteamCommand("applaunch", parameters);
        }

        private void ExecuteSteamCommand(string name, IEnumerable<string> parameters)
        {
            var arguments = new List<string>();
            arguments.Add("-" + name);
            
            if (IsSilent)
                arguments.Add("-silent");

            if (parameters != null)
                arguments.AddRange(parameters);

            var combinedArguments = string.Join(" ", arguments);

            _processProxy.Start(_steamExePath, combinedArguments);
        }
    }
}

/*
Command-line parameters
-applaunch %id %c - This launches an Game or Application through Steam. Replace the %id with the Game/Application ID number that you want to open up, replace %c with the command line parameters for the game as listed in the Source Games section above.
-cafeapplaunch - Launch apps in a cyber cafe context (Forces apps to be verified / validated before launch).
-clearbeta - Opts out of beta participation (in case for some reason it can't be done via settings).
-complete_install_via_http - Run installation completion over HTTP by default.
-console - Enables the Steam debug console tab.
-ccsyntax - Spew details about the localized strings we load.
-debug_steamapi - Enables logging of Steam API functions.
-developer - Sets the 'developer' variable to 1. Can be used to open the VGUI editor by pressing F6 or VGUI zoo by pressing F7. Intended for skin development.
-fs_log - Log file system accesses.
-fs_target - Set target syntax.
-fs_logbins - Log the binaries we load during operation.
-forceservice - Run Steam Client Service even if Steam has admin rights.
-gameoverlayinject - Sets the method how GameOverlay is injected.
-install %p - Install a product from a specified path (e.g. "D:" for the DVD-ROM drive if D: is one).
-installer_test - changes installing a retail game to emit all files to install_validate/ folder instead of to the steam cache.
-language %l - Sets the Steam language to the one specified. (Examples: "english", "german").
-login %u %p - This logs into Steam with the specified Username and Password combination. Replace %u with the username, and %p with the password you want to login with (Steam must be off for this to work).
-lognetapi - logs all P2P networking info to log/netapi_log.txt.
-log_voice - writes voice chat data to the logs/voice_log.txt file.
-noasync - Don't use async file operations, run them synchronous instead.
-nocache - This starts steam with no cache (Steam must be off for this to work properly).
-no-dwrite - forces vgui to use GDI text even if DWrite support is available.
-script %s - This runs a Steam script. Replace %s with the script filename. All scripts must be in a subdirectory of the Steam folder called test scripts (Steam must be off for this to work).
-shutdown - This shuts down (exits) Steam.
-silent - This suppresses the dialog box that opens when you start steam. It is used when you have Steam set to auto-start when your computer turns on. (Steam must be off for this to work).
-single_core - Force Steam to run on your primary CPU only.
-tcp - forces connection to Steam backend to be via TCP.
-voice_quality - sets audio quality, range [1,3].
-voicerelay - Only allow 'relay' connections for voice (testing).
*/