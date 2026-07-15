using BackupAgent.Agent.Services;

namespace BackupAgent.Agent
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            AgentHost agent = new AgentHost();

            agent.Run();
        }
    }
}