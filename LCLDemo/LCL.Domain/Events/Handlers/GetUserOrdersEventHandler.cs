﻿using LCL.Domain.Events;
using LCL.Domain.Model;
using LCL.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCL.Domain.Events.Handlers
{
    [HandlesAsynchronously]
    public class GetUserOrdersEventHandler : IDomainEventHandler<GetUserOrdersEvent>
    {
        private readonly ISalesOrderRepository salesOrderRepository;

        public GetUserOrdersEventHandler(ISalesOrderRepository salesOrderRepository)
        {
            this.salesOrderRepository = salesOrderRepository;
        }

        public void Handle(GetUserOrdersEvent evnt)
        {
            var user = evnt.Source as User;
            evnt.SalesOrders = this.salesOrderRepository.FindSalesOrdersByUser(user);
        }
    }
}
