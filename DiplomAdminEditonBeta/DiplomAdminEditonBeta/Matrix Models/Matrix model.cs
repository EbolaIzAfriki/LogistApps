using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomAdminEditonBeta.Matrix_Models
{
    public class MatrixModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int Columns { get; set; } = 0;
        public int Rows { get; set; } = 0;
        public ObservableCollection<MatrixModelItem> Items { get; set; }
        public MatrixModel()
        {
            Items = new ObservableCollection<MatrixModelItem>();
            RestorMatrix();
        }

        void OnColumnsChanged() => RestorMatrix();
        void OnRowsChanged() => RestorMatrix();

        public void RestorMatrix()
        {
            Items.Clear();
            var source = Enumerable.Range(1, Rows).SelectMany(row => Enumerable.Range(1, Columns).Select(column => new MatrixModelItem { Value = "" }));
            foreach (var item in source)
            {
                Items.Add(item);
            }
        }
    }

    public class MatrixModelItem
    {
        public string Value { get; set; }
        public bool IsNotEnable { get; set; }
    }
}
