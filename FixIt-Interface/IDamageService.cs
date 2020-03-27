using FixIt_Dto.Dto;
using FixIt_Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FixIt_Interface
{
    public interface IDamageService
    {
        List<Issue> GetDamageList();
        List<Issue> GetDamageByCategoryName(string categoryName);
    }
}
