using LCL;
/*******************************************************  
*   
* ���ߣ�������  
* ˵����  
* ���л�����.NET 4.5.0  
* �汾�ţ�1.0.0  
*   
* ��ʷ��¼��  
*    �����ļ� ������ 20154��18�� ������ 
*   
*******************************************************/
using LCL.Repositories;
using LCL.Repositories.EntityFramework;
using LCL.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;  
  
namespace UIShell.RbacPermissionService  
{  
    public interface IDictionaryRepository : IRepository<Dictionary>  
    {
        List<Dictionary> GetDictTypeList(Guid dicTypeId);
    }  
    public class DictionaryRepository : EntityFrameworkRepository<Dictionary>, IDictionaryRepository  
    {  
        public DictionaryRepository(IRepositoryContext context)  
            : base(context)  
        {   
          
        }

        public List<Dictionary> GetDictTypeList(Guid dicTypeId)
        {
            ISpecification<Dictionary> spec =
             Specification<Dictionary>.Eval(p => p.DictType.ID == dicTypeId);
            var list = this.FindAll(spec, e => e.UpdateDate, SortOrder.Descending);
            return list.ToList();
        }
    }  
}  
