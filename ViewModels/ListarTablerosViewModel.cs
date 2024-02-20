using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kanban.Models;

namespace Kanban.ViewModels;

public class ListarTablerosViewModel
{
    private List<TableroViewModel> tablerosVM;
    private List<TableroViewModel> tablerosPropiosVM;

    public ListarTablerosViewModel()
    {
    }

    public ListarTablerosViewModel(List<Tablero> tableros)
    {
        tablerosVM = new List<TableroViewModel>();
        tablerosPropiosVM = new List<TableroViewModel>();

        foreach (var tablero in tableros)
        {
            TableroViewModel tableroVM = new TableroViewModel(tablero);
            tablerosVM.Add(tableroVM);
        }
    }

    public ListarTablerosViewModel(List<Tablero> tableros, List<Tablero> tablerosPropios)
    {
        tablerosVM = new List<TableroViewModel>();
        tablerosPropiosVM = new List<TableroViewModel>();

        foreach (var tablero in tableros)
        {
            TableroViewModel tableroVM = new TableroViewModel(tablero);
            tablerosVM.Add(tableroVM);
        }

        foreach (var tablero in tablerosPropios)
        {
            TableroViewModel tableroVM = new TableroViewModel(tablero);
            tablerosPropiosVM.Add(tableroVM);
        }
    }

    public List<TableroViewModel> TablerosVM { get => tablerosVM; set => tablerosVM = value; }
    public List<TableroViewModel> TablerosPropiosVM { get => tablerosPropiosVM; set => tablerosPropiosVM = value; }
}