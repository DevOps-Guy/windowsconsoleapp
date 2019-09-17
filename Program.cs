using System;

using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System.Collections.Generic;

namespace TFS_Token_Access
{
    class Program
    {
        static void Main(string[] args)
        {

            Uri orgUrl = new Uri("https://dev.azure.com/raman9900");         // Organization URL, for example: https://dev.azure.com/fabrikam                
          // String personalAccessToken = "2ukhabsogznygn32rkegpo7jtk5kcwmljrqndzyyytma2g4r2tla";  // See https://docs.microsoft.com/azure/devops/integrate/get-started/authentication/pats 
          //  string authInfo = "carsten:Work@123";  // Enter your user name and password. If you are not using PAT, then use this method. I used this method. 
        // string _credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authInfo)); //needed if you are passing user name and password


            string project = "MyFirstProject";

            Wiql wiql = new Wiql()
            {
                Query = "Select [State], [Title] " +
                        "From WorkItems " +
                        "Where [Work Item Type] = 'Bug' " +
                        "And [System.TeamProject] = '" + project + "' " +
                        "And [System.State] <> 'Closed' " +
                        "Order By [State] Asc, [Changed Date] Desc"
            };
            // Create a connection

           // VssCredentials credentials = new VssBasicCredential("Basic", _credentials);
            VssCredentials credentials = new VssBasicCredential("carsten", "Work@123");
            //VssConnection connection = new VssConnection(orgUrl, credentials);
            WorkItemTrackingHttpClient workItemTrackingHttpClient = new WorkItemTrackingHttpClient(orgUrl, credentials);
            WorkItemQueryResult workItemQueryResult = workItemTrackingHttpClient.QueryByWiqlAsync(wiql).Result;
            //VssConnection connection = new VssConnection(orgUrl, new VssCredentials());


            // Show details a work item
            //ShowWorkItemDetails(connection, workItemId);
            Console.WriteLine(workItemQueryResult);
            Console.ReadLine();

        }

        public static void ShowWorkItemDetails(VssConnection connection, int workItemId)
        {
            // Get an instance of the work item tracking client
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

            WorkItem workitem = new WorkItem();

            // Get the specified work item
            workitem = witClient.GetWorkItemAsync(workItemId).Result;

            //workitem.Result;


            // Output the work item's field values
            foreach (var field in workitem.Fields)
            {
                Console.WriteLine("  {0}: {1}", field.Key, field.Value);
            }


        }
    }
}