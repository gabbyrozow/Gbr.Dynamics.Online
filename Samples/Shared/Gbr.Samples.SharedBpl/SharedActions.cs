using Gbr.Dynamics.Online.Operations;
using Gbr.Samples.DataModel;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gbr.Samples.SharedBpl
{
    public class SharedActions
    {
        #region Variables

        private readonly IOrganizationService serviceProxy;

        #endregion

        #region Constructor

        public SharedActions(IOrganizationService serviceProxy)
        {
            this.serviceProxy = serviceProxy;
        }

        #endregion

        #region Internal Methods

        public void CreateTask(string subject, EntityReference regardingObject)
        {
            Task task = new Task
            {
                Subject = subject,
                RegardingObjectId = regardingObject
            };
            EntityOperations entityOperations = new EntityOperations(serviceProxy);
            entityOperations.Create(task);
        }
        
        #endregion
    }
}
