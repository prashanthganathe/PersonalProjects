using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Windows8Theme.Common
{
    public class NetworkUtility
    {
       public string GetConnectionProfile(ConnectionProfile connectionProfile)
        {
            string connectionProfileInfo = string.Empty;
            if (connectionProfile != null)
            {
                connectionProfileInfo = "Profile Name : " + connectionProfile.ProfileName + "\n";

                switch (connectionProfile.GetNetworkConnectivityLevel())
                {
                    case NetworkConnectivityLevel.None:
                        connectionProfileInfo += "Connectivity Level : None\n";
                        break;
                    case NetworkConnectivityLevel.LocalAccess:
                        connectionProfileInfo += "Connectivity Level : Local Access\n";
                        break;
                    case NetworkConnectivityLevel.ConstrainedInternetAccess:
                        connectionProfileInfo += "Connectivity Level : Constrained Internet Access\n";
                        break;
                    case NetworkConnectivityLevel.InternetAccess:
                        connectionProfileInfo += "Connectivity Level : Internet Access\n";
                        break;
                }

                switch (connectionProfile.GetDomainConnectivityLevel())
                {
                    case DomainConnectivityLevel.None:
                        connectionProfileInfo += "Domain Connectivity Level : None\n";
                        break;
                    case DomainConnectivityLevel.Unauthenticated:
                        connectionProfileInfo += "Domain Connectivity Level : Unauthenticated\n";
                        break;
                    case DomainConnectivityLevel.Authenticated:
                        connectionProfileInfo += "Domain Connectivity Level : Authenticated\n";
                        break;
                }

                //Get Connection Cost information
                ConnectionCost connectionCost = connectionProfile.GetConnectionCost();
                connectionProfileInfo += GetConnectionCostInfo(connectionCost);

                //Get Dataplan Status information
                DataPlanStatus dataPlanStatus = connectionProfile.GetDataPlanStatus();
                connectionProfileInfo += GetDataPlanStatusInfo(dataPlanStatus);

            }
            return connectionProfileInfo;
        }

       string GetConnectionCostInfo(ConnectionCost connectionCost)
       {
           string cost = string.Empty;
           cost += "Connection Cost Information: \n";
           cost += "====================\n";

           if (connectionCost == null)
           {
               cost += "Connection Cost not available\n";
               return cost;
           }

           switch (connectionCost.NetworkCostType)
           {
               case NetworkCostType.Unrestricted:
                   cost += "Cost: Unrestricted";
                   break;
               case NetworkCostType.Fixed:
                   cost += "Cost: Fixed";
                   break;
               case NetworkCostType.Variable:
                   cost += "Cost: Variable";
                   break;
               case NetworkCostType.Unknown:
                   cost += "Cost: Unknown";
                   break;
               default:
                   cost += "Cost: Error";
                   break;
           }
           cost += "\n";
           cost += "Roaming: " + connectionCost.Roaming + "\n";
           cost += "Over Data Limit: " + connectionCost.OverDataLimit + "\n";
           cost += "Approaching Data Limit : " + connectionCost.ApproachingDataLimit + "\n";

           return cost;
       }

       string GetDataPlanStatusInfo(DataPlanStatus dataPlan)
       {
           string dataplanStatusInfo = string.Empty;
           dataplanStatusInfo = "Dataplan Status Information:\n";
           dataplanStatusInfo += "====================\n";

           if (dataPlan == null)
           {
               dataplanStatusInfo += "Dataplan Status not available\n";
               return dataplanStatusInfo;
           }

           if (dataPlan.DataPlanUsage != null)
           {
               dataplanStatusInfo += "Usage In Megabytes : " + dataPlan.DataPlanUsage.MegabytesUsed + "\n";
               dataplanStatusInfo += "Last Sync Time : " + dataPlan.DataPlanUsage.LastSyncTime + "\n";
           }
           else
           {
               dataplanStatusInfo += "Usage In Megabytes : Not Defined\n";
           }

           ulong? inboundBandwidth = dataPlan.InboundBitsPerSecond;
           if (inboundBandwidth.HasValue)
           {
               dataplanStatusInfo += "InboundBitsPerSecond : " + inboundBandwidth + "\n";
           }
           else
           {
               dataplanStatusInfo += "InboundBitsPerSecond : Not Defined\n";
           }

           ulong? outboundBandwidth = dataPlan.OutboundBitsPerSecond;
           if (outboundBandwidth.HasValue)
           {
               dataplanStatusInfo += "OutboundBitsPerSecond : " + outboundBandwidth + "\n";
           }
           else
           {
               dataplanStatusInfo += "OutboundBitsPerSecond : Not Defined\n";
           }

           uint? dataLimit = dataPlan.DataLimitInMegabytes;
           if (dataLimit.HasValue)
           {
               dataplanStatusInfo += "DataLimitInMegabytes : " + dataLimit + "\n";
           }
           else
           {
               dataplanStatusInfo += "DataLimitInMegabytes : Not Defined\n";
           }

           System.DateTimeOffset? nextBillingCycle = dataPlan.NextBillingCycle;
           if (nextBillingCycle.HasValue)
           {
               dataplanStatusInfo += "NextBillingCycle : " + nextBillingCycle + "\n";
           }
           else
           {
               dataplanStatusInfo += "NextBillingCycle : Not Defined\n";
           }

           uint? maxTransferSize = dataPlan.MaxTransferSizeInMegabytes;
           if (maxTransferSize.HasValue)
           {
               dataplanStatusInfo += "MaxTransferSizeInMegabytes : " + maxTransferSize + "\n";
           }
           else
           {
               dataplanStatusInfo += "MaxTransferSizeInMegabytes : Not Defined\n";
           }
           return dataplanStatusInfo;
       }

    }
}
