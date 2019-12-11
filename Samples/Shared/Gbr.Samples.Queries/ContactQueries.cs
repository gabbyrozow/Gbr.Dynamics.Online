using Gbr.Samples.DataModel;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gbr.Samples.Queries
{
    public class ContactQueries
    {
        #region Variables

        private readonly GbrSamplesContext serviceContext;

        #endregion

        #region Constructor

        public ContactQueries(IOrganizationService serviceProxy)
        {
            serviceContext = new GbrSamplesContext(serviceProxy);
        }

        #endregion

        #region Public Methods

        public List<Contact> GetContactsByParentId(Guid parentId)
        {
            return serviceContext.ContactSet.Where(c => c.ParentCustomerId.Id == parentId)
                .Select(c => new Contact
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName
                }).ToList();
        }

        #endregion
    }
}
