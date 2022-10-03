namespace SupportingElicitation.Lib
{
    internal class Oracle
    {
        internal static double GetNumberOfReqsForTemplate(Dictionary<string, double> project, string template)
        {
            return project[template];
        }

        internal static bool IsProjectUsingTemplate(Dictionary<string, double> project, string template)
        {
            return project.ContainsKey(template) && project[template] > 0;
        }
    }
}