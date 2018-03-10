using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CommonTools.Lib.ns11.DataStructures
{
    public class UIList<T> : ObservableCollection<T>
    {
        public UIList()
        {
        }

        public UIList(IEnumerable<T> rowItems)
        {
            SetItems(rowItems);
        }

        public UIList(IEnumerable<T> rowItems, T summaryRow, double summaryAmount)
        {
            SetItems       (rowItems);
            SetSummary     (summaryRow);
            SummaryAmount = summaryAmount;
        }


        public double  SummaryAmount  { get; set; }
        public T       CurrentItem    { get; set; }


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
