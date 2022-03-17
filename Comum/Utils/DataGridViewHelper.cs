using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comum.Utils
{
    public static class DataGridViewHelper
    {
        public static void ChangeShownColumns(this DataGridView grid, params string[] columnNames)
        {
            if (columnNames != null)
            {
                foreach (DataGridViewColumn column in grid.Columns)
                {
                    column.Visible = columnNames.Contains(column.Name);
                }
            }
        }

        public static void ChangeColumnsVisibility(this DataGridView grid, bool isVisible, params string[] columnNames)
        {
            if (columnNames != null)
            {
                foreach (string columnName in columnNames)
                {
                    if (grid.Columns.Contains(columnName))
                    {
                        grid.Columns[columnName].Visible = isVisible;
                    }
                }
            }
        }

        public static void ChangeColumnsOrder(this DataGridView grid, params string[] columnNames)
        {
            if (columnNames != null)
            {
                int idx = 0;

                grid.AutoGenerateColumns = false;

                foreach (string columnName in columnNames)
                {
                    if (!grid.Columns.Contains(columnName))
                    {
                        continue;
                    }

                    DataGridViewColumn column = grid.Columns[columnName];
                    grid.Columns.Remove(columnName );
                    grid.Columns.Insert(idx, column);
                    column.DisplayIndex = idx;
                    idx++;
                }
            }

            //grid.Update();
            //grid.Show();
        }

        /*
        public static void ChangeColumnsHeaderText(this DataGridView grid, bool isVisible, params string[] columnNames)
        {
            if (columnNames != null)
            {
                foreach (string columnName in columnNames)
                {
                    if (grid.Columns.Contains(columnName))
                    {
                        grid.Columns[columnName].HeaderText  = isVisible;
                    }
                }
            }
        }
        */




        /*
        public static DataGridViewExtraOptions Extra(this DataGridView grid)
        {
            return new DataGridViewExtraOptions(grid);
        }
        */


        /*
            Public Property HeaderText(ByVal columnName As String) As String
                Get
                    If Grid.Columns.Contains(columnName) Then _
                        Return Grid.Columns(columnName).HeaderText
                    Return ""
                End Get
                Set(value As String)
                    If Grid.Columns.Contains(columnName) Then _
                        Grid.Columns(columnName).HeaderText = value
                End Set
            End Property
    
            Public Property ColumnVisible(ByVal columnName As String) As Boolean
                Get
                    If Me.Grid.Columns.Contains(columnName) Then _
                        Return Me.Grid.Columns(columnName).Visible
                    Return False
                End Get
                Set(value As Boolean)
                    If Me.Grid.Columns.Contains(columnName) Then _
                        Me.Grid.Columns(columnName).Visible = value
                End Set
            End Property
        */

    }

    /*
    public class GetterFieldIndexer<T>
    {
        private Func<string, T> GetterExpression { get; set; }

        public GetterFieldIndexer(Func<string, T> getterExpression)
        {
            this.GetterExpression = getterExpression;
        }

        public T this[string columnName]
        {
            get
            {
                return this.GetterExpression.Invoke(columnName);
            }
        }
    }

    public class GetterAndSetterFieldIndexer<T> 
    {
        private Func<string, T> GetterExpression { get; set; }
        private Action<string, T> SetterExpression { get; set; }

        public GetterAndSetterFieldIndexer(Func<string, T> getterExpression, Action<string, T> setterExpression)
        {
            this.SetterExpression = setterExpression;
            this.GetterExpression = getterExpression;
        }

        public T this[string columnName]
        {
            get
            {
                return this.GetterExpression.Invoke(columnName);
            }
            set
            {
                this.SetterExpression.Invoke(columnName, value);
            }
        }
    }

    public class DataGridViewExtraOptions
    {
        protected DataGridView Grid { get; set; }

        public DataGridViewExtraOptions(DataGridView grid)
        {
            this.Grid = grid;
        }


        public GetterAndSetterFieldIndexer<string> HeaderText
        {
            get
            {
                return new GetterAndSetterFieldIndexer<string>(
                        columnName =>
                        {
                            if (this.Grid.Columns.Contains(columnName))
                            {
                                return this.Grid.Columns[columnName].HeaderText;
                            }
                            return string.Empty;
                        },
                        (columnName, value) =>
                        {
                            if (this.Grid.Columns.Contains(columnName))
                            {
                                this.Grid.Columns[columnName].HeaderText = value;
                            }
                        }
                    );
            }
        }

        public string GetHeaderText(string columnName)
        {
            if (this.Grid.Columns.Contains(columnName))
            {
                return this.Grid.Columns[columnName].HeaderText;
            }

            return string.Empty;
        }

        public void SetHeaderText(string columnName, string titleText)
        {
            if (this.Grid.Columns.Contains(columnName))
            {
                this.Grid.Columns[columnName].HeaderText = titleText;
            }
        }

        public void SetColumnVisible(string columnName, bool value)
        {
            if( this.Grid.Columns.Contains(columnName) )
            {
                this.Grid.Columns[columnName].Visible = value;
            }
        }

        public bool GetColumnVisible(string columnName)
        {
            if (this.Grid.Columns.Contains(columnName))
            {
                return this.Grid.Columns[columnName].Visible;
            }

            return false;
        }
    }
    */


    
}