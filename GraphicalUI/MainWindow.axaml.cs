using Avalonia.Controls;
using System;
using System.Linq;
using GarageApp;

namespace GraphicalUI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        VehicleTypeComboBox.ItemsSource =
            Enum.GetValues<Vehicle.VehicleTypeE>();

        AddVehicleButton.Click += AddVehicle;
        RemoveVehicleButton.Click += RemoveVehicle;
        SaveButton.Click += SaveInventory;
        LoadButton.Click += LoadInventory;
    }

    private void AddVehicle(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        StatusText.Text = "Vehicle added.";
    }

    private void RemoveVehicle(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        StatusText.Text = "Vehicle removed.";
    }

    private void SaveInventory(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        StatusText.Text = "Inventory saved.";
    }

    private void LoadInventory(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        StatusText.Text = "Inventory loaded.";
    }
}