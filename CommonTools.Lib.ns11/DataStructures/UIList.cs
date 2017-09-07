using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CommonTools.Lib.ns11.DataStructures
{
    public class UIList<T> : ObservableCollection<T>
    {
        //private      EventHandler _summaryRowAdded;
        //public event EventHandler  SummaryRowAdded
        //{
        //    add    { _summaryRowAdded -= value; _summaryRowAdded += value; }
        //    remove { _summaryRowAdded -= value; }
        //}


        //public UIList()
        //{
        //    SummaryRow.CollectionChanged += (s, e) 
        //        => _summaryRowAdded?.Raise();
        //}


        public ObservableCollection<T>  SummaryRows  { get; } = new ObservableCollection<T>();


        public void SetItems(IEnumerable<T> items)
        {
            this.Clear();

            foreach (var item in items)
                this.Add(item);
        }


        public void SetSummary(T summaryRow)
        {
            SummaryRows.Clear();
            SummaryRows.Add(summaryRow);
        }
    }
}
