using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;

namespace Kanban.ViewModels;

public class ListarTablerosViewModel
{
    List<TableroViewModel> tablerosVM;

    public ListarTablerosViewModel(List<Tablero> tableros)
    {
        tablerosVM = new List<TableroViewModel>();

        foreach (var tablero in tableros)
        {
            TableroViewModel tableroNuevo = new TableroViewModel(tablero);
            tablerosVM.Add(tableroNuevo);
        }
    }

    public List<TableroViewModel> TablerosVM { get => tablerosVM; set => tablerosVM = value; }
}