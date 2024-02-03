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

    public ListarTablerosViewModel(List<TableroViewModel> tableros, List<TableroViewModel> tablerosPropios)
    {
        tablerosVM = new List<TableroViewModel>();
        tablerosPropiosVM = new List<TableroViewModel>();

        foreach (var tablero in tableros)
        {
            tablerosVM.Add(tablero);
        }

        foreach (var tablero in tablerosPropios)
        {
            tablerosPropiosVM.Add(tablero);
        }
    }

    public List<TableroViewModel> TablerosVM { get => tablerosVM; set => tablerosVM = value; }
    public List<TableroViewModel> TablerosPropiosVM { get => tablerosPropiosVM; set => tablerosPropiosVM = value; }
}