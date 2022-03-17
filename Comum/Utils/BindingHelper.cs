using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using System.ComponentModel;
using System.Windows.Forms;
using Comum.Models;

namespace Comum.Utils
{
    /*
    public class DataBinding
    {
        private Type ControlType { get; set; }
        private Type DataSourceType { get; set; }

        public Control Control {get;set;}
        public string ControlPropertyName { get; set; }
        public object DataSource { get; set; }
        public string DataSourcePropertyName { get; set; }
        public Expression Expression { get; set; }

        public DataBinding(Control control, string controlPropertyName, object dataSource, string dataSourcePropertyName)
        {
            this.Control = control;
            this.ControlPropertyName = controlPropertyName;
            this.DataSource = dataSource;
            this.DataSourcePropertyName = dataSourcePropertyName;

            this.ControlType = this.Control.GetType();
            this.DataSourceType = this.DataSourceType.GetType();
        }

        public void Bind()
        {
            PropertyInfo controlInfo = this.ControlType.GetProperty(this.ControlPropertyName);
            PropertyInfo dataSourceInfo = this.DataSourceType.GetProperty(this.DataSourcePropertyName);


            // Binding the "Changing event":
            EventInfo changingInfo = this.DataSourceType.GetEvent("PropertyChangingEventHandler");
            if (changingInfo != null)
            {
                changingInfo.AddEventHandler(dataSourceInfo, new PropertyChangingEventHandler(this.FromDataSourceChangingDelegated));
            }


            // Binding the "Changed event":
            EventInfo changedInfo = this.DataSourceType.GetEvent("PropertyChangedEventHandler");
            if (changedInfo != null)
            {
                changedInfo.AddEventHandler(dataSourceInfo, new PropertyChangedEventHandler(this.FromDataSourceChangedDelegated));
            }
        }

        private void FromDataSourceChangingDelegated(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == this.DataSourcePropertyName)
            {
                INotifyPropertyChanging observable = this.DataSource as INotifyPropertyChanging;
                if (observable != null )
                {
                    if (observable.PropertyChanging != null)
                    {
                    }
                }
            }
        }

        private void FromDataSourceChangedDelegated(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.DataSourcePropertyName)
            {

            }
        }

        

        public void UnBind()
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return this.Control.GetHashCode() 
                    * this.ControlPropertyName.GetHashCode() 
                    * this.DataSource.GetHashCode() 
                    * this.DataSourcePropertyName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            DataBinding other = obj as DataBinding;

            if (other == null)
                return false;

            if (object.ReferenceEquals(this.Control, other.Control)
                && object.ReferenceEquals(this.DataSource, other.DataSource)
                && this.ControlPropertyName == other.ControlPropertyName
                && this.DataSourcePropertyName == other.DataSourcePropertyName)
            {
                return true;
            }

            return false;
        }
    }
    */
    

    public static class BindingHelper
    {
        /*
        private static HashSet<DataBinding> Controls = new HashSet<DataBinding>();
        private static Dictionary<Control, DataBinding> ControlPairs = new Dictionary<Control, DataBinding>();
        
        public static void Bind(Control control, string controlPropertyName, object dataSource, string dataSourcePropertyName)
        {
            DataBinding binding = new DataBinding(control, controlPropertyName, dataSource, dataSourcePropertyName);

            if (!Controls.Contains(binding))
            {
                binding.Bind();
                Controls.Add(binding);
            }
        }
        */


        private static string GetMemberName(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var memberExpression = (MemberExpression)expression;
                    var supername = GetMemberName(memberExpression.Expression);
                    if (String.IsNullOrEmpty(supername)) return memberExpression.Member.Name;
                    return String.Concat(supername, '.', memberExpression.Member.Name);
                case ExpressionType.Call:
                    var callExpression = (MethodCallExpression)expression;
                    return callExpression.Method.Name;
                case ExpressionType.Convert:
                    var unaryExpression = (UnaryExpression)expression;
                    return GetMemberName(unaryExpression.Operand);
                case ExpressionType.Parameter:
                case ExpressionType.Constant: //Change
                    return String.Empty;
                default:
                    throw new ArgumentException("The expression is not a member access or method call expression");
            }
        }

        public static string Name<T, T2>(Expression<Func<T, T2>> expression)
        {
            return GetMemberName(expression.Body);
        }

        //NEW
        public static string Name<T>(Expression<Func<T>> expression)
        {
            return GetMemberName(expression.Body);
        }

        public static void Bind<TC, TD, TP>(this TC control, Expression<Func<TC, TP>> controlProperty, TD dataSource, Expression<Func<TD, TP>> dataMember) where TC : Control
        {
            control.DataBindings.Add(Name(controlProperty), dataSource, Name(dataMember));
        }

        public static void BindLabelText<T>(this Label control, T dataObject, Expression<Func<T, object>> dataMember)
        {
            // as this is way one any type of property is ok
            control.DataBindings.Add("Text", dataObject, Name(dataMember));
        }

        public static void BindEnabled<T>(this Control control, T dataObject, Expression<Func<T, bool>> dataMember)
        {
            control.Bind(c => c.Enabled, dataObject, dataMember);
        }
    }
}