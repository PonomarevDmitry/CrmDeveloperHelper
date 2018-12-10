using System;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    public class TupleList<T1, T2> : List<Tuple<T1, T2>>
    {
        public void Add(T1 t1, T2 t2)
        {
            this.Add(Tuple.Create(t1, t2));
        }
    }

    public class TupleList<T1, T2, T3> : List<Tuple<T1, T2, T3>>
    {
        public void Add(T1 t1, T2 t2, T3 t3)
        {
            this.Add(Tuple.Create(t1, t2, t3));
        }
    }

    public class TupleList<T1, T2, T3, T4> : List<Tuple<T1, T2, T3, T4>>
    {
        public void Add(T1 t1, T2 t2, T3 t3, T4 t4)
        {
            this.Add(Tuple.Create(t1, t2, t3, t4));
        }
    }

    public class TupleList<T1, T2, T3, T4, T5> : List<Tuple<T1, T2, T3, T4, T5>>
    {
        public void Add(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            this.Add(Tuple.Create(t1, t2, t3, t4, t5));
        }
    }

    public class TupleList<T1, T2, T3, T4, T5, T6> : List<Tuple<T1, T2, T3, T4, T5, T6>>
    {
        public void Add(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            this.Add(Tuple.Create(t1, t2, t3, t4, t5, t6));
        }
    }

    public class TupleList<T1, T2, T3, T4, T5, T6, T7> : List<Tuple<T1, T2, T3, T4, T5, T6, T7>>
    {
        public void Add(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            this.Add(Tuple.Create(t1, t2, t3, t4, t5, t6, t7));
        }
    }
}