using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tunynet.Common;
using Tunynet.Common.Repositories;

namespace Tunynet.tests
{
    public class SchoolRepositoryUtility
    {
        private SchoolRepository schoolRepository;

        public SchoolRepositoryUtility(SchoolRepository SchoolRepository) {
            this.schoolRepository = SchoolRepository;
        }

        public long Insert(int displayOrder)
        {
            School school = School.New();
            return 0;
        }
    }
}
