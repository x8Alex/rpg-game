using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace boardProto
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Launches resolution selection screen
            ResolutionQuery resolutionQuery = new ResolutionQuery();
            Application.Run(resolutionQuery);

            // Executes if the form returns true for LAUNCHGAME
            if (resolutionQuery.getLaunchGame())
            {
                using (var game = new Game1(resolutionQuery.getResolution()))
                    game.Run();
            }
        }
    }
#endif
}