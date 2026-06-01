using AXIOCRM.Application.DTOs;
using AXIOCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.Mappings
{
    public static class ClientMappingExtensions
    {

        public static ClientDto ToDto(this Client c)
        {
            if (c == null) return null;

            return new ClientDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Address
            };

        }


        }
}
