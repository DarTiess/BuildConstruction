using BuildingObjects;
using CameraFollow;
using Data;
using Grid;
using Messenger;
using SaveService;
using UI;
using UnityEngine;
using Zenject;

public class Bootstrap: MonoBehaviour
{
    [SerializeField] private Transform _canvas;
    [SerializeField] private Transform _gridOrigin;
    [SerializeField] private Building _buildPrefab;
    [SerializeField] private int _poolSize;
    [SerializeField] private Transform _buildContainer;
    private UIRoot _uiRoot;
    private CamFollow _camera;
    private GroundSettings _groundSettings;
    private IMediatorFactory _mediatorFactory;
    private IMessenger _messenger;
    private IBuildFactory _buildFactory;
    private IGroundFactory _groundFactory;
    private IPersistentData _persistentData;

    [Inject]
    public void Construct(IMediatorFactory mediatorFactory, IMessenger messenger,
        GroundSettings groundSettings, IBuildFactory buildFactory, IGroundFactory groundFactory,
        IPersistentData persistentData)
    {
        _mediatorFactory = mediatorFactory;
        _messenger = messenger;
        _groundSettings = groundSettings;
        _buildFactory = buildFactory;
        _groundFactory = groundFactory;
        _persistentData = persistentData;
    }
    private void Awake()
    {
        InitPersistenData();
        CreateUiRoot();
        SetCameraPosition();
        CreateGroundFactory();
        InitBuildFactory();
    }

    private void InitPersistenData()
    {
        _persistentData.Initialize();
    }

    private void SetCameraPosition()
    {
        _camera = Camera.main.GetComponent<CamFollow>();
        _camera.Init(_groundSettings.GridSize);
    }

    private void CreateGroundFactory()
    {
        _groundFactory.Init(_gridOrigin);
    }

    private void CreateUiRoot()
    {
        _mediatorFactory.Init(_canvas, _messenger);
        _uiRoot = new UIRoot(_mediatorFactory);
        _uiRoot.Init();
    }

    private void InitBuildFactory()
    {
        _buildFactory.Init(_buildPrefab, _poolSize, _buildContainer, Camera.main);
    }
}