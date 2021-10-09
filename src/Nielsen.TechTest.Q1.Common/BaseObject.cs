using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Nielsen.TechTest.Q1.Common
{
    public abstract class BaseObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> exp)
        {
            var member = exp.Body as MemberExpression;
            if (member == null || member.Member == null || String.IsNullOrWhiteSpace(member.Member.Name))
            {
                return;
            }
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(member.Member.Name));
        }
    }
}
