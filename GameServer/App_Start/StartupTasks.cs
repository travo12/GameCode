using Planc.Dal;

namespace Planc
{
    public static class StartupTasks
    {
        public static void Run()
        {
            //Initialized Database
            GameConstants.Dal.InitializeDb();
        }
    }
}