using UnityEngine;
using WorldGraphEditor;
using Zenject;

namespace ZenjectDemo
{
    [RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class ZenjectPassage : MonoBehaviour, ITransitionComponent, IPusher
    {
        [SerializeField] private PortsDropdown _port;
        [SerializeField] private Optional<Vector2> _pushForce;
        
        [SerializeField, HideInInspector] private BoxCollider2D _boxCollider;
        [SerializeField, HideInInspector] private SpriteRenderer _spriteRenderer;

        private bool _canBeUsed;
        private ITransitionManager _manager;
        
        [Inject]
        private void Construct(ITransitionManager manager)
        {
            _manager = manager;
        }

        private void OnValidate()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _boxCollider.isTrigger = true;

            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (this.IsShortcutDestination() || this.IsOneWayDestination())
            {
                _spriteRenderer.color = new Color(1f, 0.36f, 0.35f);
                return;
            }

            if (this.IsShortcutOrigin() || this.IsOneWayOrigin())
            {
                _spriteRenderer.color = new Color(0.44f, 1f, 0.37f);
                return;
            }
            
            _spriteRenderer.color = new Color(0.4f, 0.79f, 1f);
        }

#if UNITY_EDITOR
        public void Refresh(RefreshContext context)
        {
            context.FillPortsDropdownData(_port);
        }
#endif

        public Vector3 GetSpawnPosition()
        {
            return transform.position;
        }

        public string GetGuid()
        {
            return _port.GetSelectedValue();
        }

        public PushData GetPushData()
        {
            return _pushForce.Enabled ? new PushData(_pushForce.Value) : default;
        }
        
        public void Traverse()
        {
            _manager.GoFromAsync(GetGuid());
        }
        
        private void Awake()
        {
            _canBeUsed = true;
        }
        
        private void OnEnable()
        {
            _manager.SceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            _manager.SceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded()
        {
            if (this.IsOutput(_manager))
                _canBeUsed = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_canBeUsed)
                Traverse();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _canBeUsed = true;
        }
    }
}