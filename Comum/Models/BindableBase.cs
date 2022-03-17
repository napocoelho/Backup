using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using WeakEvent;

namespace Comum.Models
{
    /*
    public interface IBindableBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        event PropertyChangedEventHandler PropertyChanged;
        event PropertyChangingEventHandler PropertyChanging;

        //void OnPropertyChanged([CallerMemberName] string propertyName = null);
        //void OnPropertyChanging([CallerMemberName] string propertyName = null);
    }
    */

    [Serializable]
    public abstract class BindableBase : INotifyPropertyChanged, INotifyPropertyChanging    // IBindableBase
    {
        [NonSerialized()]
        private WeakPropertyChangedSource _changedEventSource;

        [NonSerialized()]
        private WeakPropertyChangingSource _changingEventSource;

        
        private WeakPropertyChangedSource ChangedEventSource { get { return _changedEventSource == null ? new WeakPropertyChangedSource() : _changedEventSource;  } }
        
        private WeakPropertyChangingSource ChangingEventSource { get { return _changingEventSource == null ? new WeakPropertyChangingSource() : _changingEventSource; } }


        public event PropertyChangedEventHandler PropertyChanged
        {
            add => this.ChangedEventSource.Subscribe(value);
            remove => this.ChangedEventSource.Unsubscribe(value);
        }

        public event PropertyChangingEventHandler PropertyChanging
        {
            add => this.ChangingEventSource.Subscribe(value);
            remove => this.ChangingEventSource.Unsubscribe(value);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.ChangedEventSource.Raise(this, new PropertyChangedEventArgs(propertyName));

            /*
            if (PropertyChanged != null)
            {
                lock (string.Intern(propertyName))
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
            */
        }

        protected void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            this.ChangingEventSource.Raise(this, new PropertyChangingEventArgs(propertyName));

            /*
            if (PropertyChanging != null)
            {
                lock (string.Intern(propertyName))
                {
                    PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
                }
            }
            */
        }

        
        
        /*
        protected void BindPropertyChanges<T>(BindableBase from, Expression<Func<T, object>> expr)
        {
            string propertyName = ObterNome<T>(expr);

            from.PropertyChanging += (sender, evt) =>
                {
                    if (evt.PropertyName == propertyName)
                    {
                        OnPropertyChanging(propertyName);
                    }
                };

            from.PropertyChanged += (sender, evt) =>
                {
                    if (evt.PropertyName == propertyName)
                    {
                        OnPropertyChanged(propertyName);
                    }
                };
        }

        private static string GetPropertyName<T>(Expression<Func<object>> expr) //Expression<T> expr)
        {
            UnaryExpression memberExpr = expr.Body as UnaryExpression; // parse da expressao "x.y"
            MemberExpression operand = memberExpr.Operand as MemberExpression; // parse do operando "y"
            return operand.Member.Name; // parse do nome do operando "y"
        }

        */

        public String ObterNome<TIn>(Expression<Func<TIn, object>> expression)
        {
            return ObterNome(expression as Expression);
        }

        public String ObterNome<TOut>(Expression<Func<TOut>> expression)
        {
            return ObterNome(expression as Expression);
        }

        private String ObterNome(Expression expression)
        {
            var lambda = expression as LambdaExpression;
            var member = lambda.Body.NodeType == ExpressionType.Convert
                                ? ((UnaryExpression)lambda.Body).Operand as MemberExpression
                                : lambda.Body as MemberExpression;
            return member.Member.Name;
        }
        

    }
}