using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Errors
{
    public class ErrorList : IEnumerable<Error>
    {
        private readonly List<Error> _errors;

        public ErrorList(IEnumerable<Error> errors)
        {
            _errors = [..errors];
        }


        public IEnumerator<Error> GetEnumerator() => _errors.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
