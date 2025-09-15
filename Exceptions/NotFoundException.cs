using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_album.Exceptions
{
    public class NotFoundException : BaseException
    {
        public override string Message { get; }

        public NotFoundException(string fieldDescription)
        {
            this.Message = $"{fieldDescription} 資料不存在!";
        }
    }
}