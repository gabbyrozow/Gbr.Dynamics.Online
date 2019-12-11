using Gbr.Samples.DataModel;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gbr.Samples.Queries
{
    public class TaskQueries
    {
        #region Variables

        private readonly GbrSamplesContext serviceContext;

        #endregion

        #region Constructor

        public TaskQueries(IOrganizationService serviceProxy)
        {
            serviceContext = new GbrSamplesContext(serviceProxy);
        }

        #endregion

        #region Public Methods

        public List<Task> GetTasksByAccountId(Guid accountId)
        {
            return serviceContext.TaskSet.Where(c => c.RegardingObjectId.Id == accountId)
                .Select(t => new Task
                {
                    Id = t.Id,
                    Subject = t.Subject
                }).ToList();
        }

        #endregion
    }
}
