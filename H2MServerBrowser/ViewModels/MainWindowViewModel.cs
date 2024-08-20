using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using H2MServerBrowser.Models;
using H2MServerBrowser.Services;

namespace H2MServerBrowser.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ServerService _serverService;
    private string _hostnameFilter = "";
    private string _ipFilter = "";
    private string _portFilter = "";
    private string _gametypeFilter = "";
    private string _mapFilter = "";
    private string _currentClientCountFilter = "";
    private string _maxClientCountFilter = "";
    private string _pingFilter = "";

    public MainWindowViewModel(ServerService serverService)
    {
        _serverService = serverService;
        ServerDataList = new ObservableCollection<ServerData>();
        FilteredServerDataList = new ObservableCollection<ServerData>();
        _ = FetchDataAsync();
    }

    public MainWindowViewModel()
    {
        _serverService = new ServerService();
        ServerDataList = new ObservableCollection<ServerData>();
        FilteredServerDataList = new ObservableCollection<ServerData>();
    }

    public ObservableCollection<ServerData> ServerDataList { get; }
    public ObservableCollection<ServerData> FilteredServerDataList { get; }

    public string HostnameFilter
    {
        get => _hostnameFilter;
        set
        {
            _hostnameFilter = value;
            OnPropertyChanged();
            ApplyFilters();
        }
    }

    public string IpFilter
    {
        get => _ipFilter;
        set
        {
            _ipFilter = value;
            OnPropertyChanged();
            ApplyFilters();
        }
    }

    public string PortFilter
    {
        get => _portFilter;
        set
        {
            _portFilter = value;
            OnPropertyChanged();
            ApplyFilters();
        }
    }

    public string GametypeFilter
    {
        get => _gametypeFilter;
        set
        {
            _gametypeFilter = value;
            OnPropertyChanged();
            ApplyFilters();
        }
    }

    public string MapFilter
    {
        get => _mapFilter;
        set
        {
            _mapFilter = value;
            OnPropertyChanged();
            ApplyFilters();
        }
    }

    public string CurrentClientCountFilter
    {
        get => _currentClientCountFilter;
        set
        {
            _currentClientCountFilter = value;
            OnPropertyChanged();
            ApplyFilters();
        }
    }

    public string MaxClientCountFilter
    {
        get => _maxClientCountFilter;
        set
        {
            _maxClientCountFilter = value;
            OnPropertyChanged();
            ApplyFilters();
        }
    }

    public string PingFilter
    {
        get => _pingFilter;
        set
        {
            _pingFilter = value;
            OnPropertyChanged();
            ApplyFilters();
        }
    }

    private async Task FetchDataAsync()
    {
        var data = await _serverService.FetchServerDataAsync("https://master.iw4.zip/servers");
        ServerDataList.Clear();
        foreach (var item in data)
        {
            ServerDataList.Add(item);
        }
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filtered = ServerDataList.Where(server =>
            (server.Hostname != null && (string.IsNullOrEmpty(HostnameFilter) || server.Hostname.Contains(HostnameFilter, StringComparison.OrdinalIgnoreCase))) &&
            (server.Ip != null && (string.IsNullOrEmpty(IpFilter) || server.Ip.Contains(IpFilter, StringComparison.OrdinalIgnoreCase))) &&
            (server.Port != null && (string.IsNullOrEmpty(PortFilter) || server.Port.ToString()!.Contains(PortFilter))) &&
            (server.Gametype != null && (string.IsNullOrEmpty(GametypeFilter) || server.Gametype.Contains(GametypeFilter, StringComparison.OrdinalIgnoreCase))) &&
            (server.Map != null && (string.IsNullOrEmpty(MapFilter) || server.Map.Contains(MapFilter, StringComparison.OrdinalIgnoreCase))) &&
            (server.CurrentClientCount != null && (string.IsNullOrEmpty(CurrentClientCountFilter) || server.CurrentClientCount.ToString()!.Contains(CurrentClientCountFilter))) &&
            (server.MaxClientCount != null && (string.IsNullOrEmpty(MaxClientCountFilter) || server.MaxClientCount.ToString()!.Contains(MaxClientCountFilter))) &&
            ((string.IsNullOrEmpty(PingFilter) || server.PingResult.ToString().Contains(PingFilter)))
        ).ToList();

        FilteredServerDataList.Clear();
        foreach (var item in filtered)
        {
            FilteredServerDataList.Add(item);
        }
    }

    private new void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public new event PropertyChangedEventHandler? PropertyChanged;
}