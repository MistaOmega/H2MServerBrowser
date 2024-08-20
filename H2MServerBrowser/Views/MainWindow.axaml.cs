using System;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using H2MServerBrowser.Models;

namespace H2MServerBrowser.Views;

public partial class MainWindow : Window
{
    private INotificationManager _notificationManager;
    public MainWindow()
    {
        InitializeComponent();
        ServerGrid.CellPointerPressed += ServerGrid_CellPointerPressed;
        _notificationManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.TopRight,
            MaxItems = 3
        };
    }
    private void ServerGrid_CellPointerPressed(object? sender, DataGridCellPointerPressedEventArgs e)
    {
        if (!e.PointerPressedEventArgs.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
        if (e.Row.DataContext is not ServerData serverData) return;
        var textToCopy = serverData.ToString();
        var clipboard = GetTopLevel(this)?.Clipboard;
        clipboard?.SetTextAsync(textToCopy);
        ShowNotification($"Copied server data to clipboard \n{serverData}");
    }

    private void ShowNotification(string message)
    {
        var notification = new Notification()
        {
            Title = "Copied Text",
            Message = message,
            Type = NotificationType.Information,
            Expiration = TimeSpan.FromSeconds(2),
        };
        _notificationManager.Show(notification);
    }
}