using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public partial class ViewPager : MonoBehaviour, IInitializable
{
    public event Action<int> PageChanged; 
//    [SerializeField] private RectTransform _content;
    [SerializeField] private List<RectTransform> _pages = new List<RectTransform>();
    [SerializeField] private float _space;
    [SerializeField] private Direction _moveDirection;
    [SerializeField] private Button _nextBtn;
    [SerializeField] private Button _previousBtn;
    private int _currentPage;

    public bool Initialized { get; private set; }

    public Content PageContent { get; private set; }

    public int CurrentPage
    {
        get { return _currentPage; }
        private set
        {
            if (_currentPage == value)
            {
                return;
            }

            _currentPage = value;
            OnPageChanged();
            PageChanged?.Invoke(CurrentPage);
        }
    }


    private bool IsHorizontal => _moveDirection == Direction.LeftToRight || _moveDirection == Direction.RightToLeft;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (Initialized)
        {
            return;
        }

        var currentPosition = 0f;

        for (var i = 0; i < _pages.Count; i++)
        {
            var page = _pages[i];

            if (_moveDirection == Direction.LeftToRight || _moveDirection == Direction.RightToLeft)
            {
                page.transform.position = transform.position +
                                          ( _moveDirection == Direction.LeftToRight ? transform.right : -transform.right) *
                                          (currentPosition + (i > 0 ? page.GetSizeInScreenSpace().x / 2 : 0));
                currentPosition += (i > 0 ? page.GetSizeInScreenSpace().x : page.GetSizeInScreenSpace().x / 2) + _space;
            }
            else
            {
                page.transform.position = transform.position +
                                          (_moveDirection == Direction.TopToBottom ? -transform.up : transform.up) *
                                          (currentPosition + (i > 0 ? page.GetSizeInScreenSpace().y / 2 : 0));
                currentPosition += (i > 0 ? page.GetSizeInScreenSpace().y : page.GetSizeInScreenSpace().y / 2) + _space;
            }
        }

        PageContent = new Content(_pages, Vector2.zero);
        CurrentPage = 0;
        _nextBtn?.onClick.AddListener(() =>
        {
            if(CurrentPage >= _pages.Count-1)
                return;

            SetPageIndex(CurrentPage+1);
        });
        _previousBtn?.onClick.AddListener(() =>
        {
            if(CurrentPage<=0)
                return;

            SetPageIndex(CurrentPage - 1);
        });
        Initialized = true;
    }

    public void SetPageIndex(int index, bool animate = true)
    {
        StopAutoMovingIfMove();
        CurrentPage = index;
        if (animate)
        {
            StartMovingToCurrentPage();
        }
        else
        {
            PageContent.Position = -GetPositionForPage(index);
        }

    }

    public Vector2 GetPositionForPage(int index)
    {
        return -PageContent.Position + (Vector2) _pages[index].localPosition;
    }

    public int GetNearestPage()
    {
        Debug.Log(nameof(GetNearestPage));
        var currentPage = _pages.OrderBy(rectTransform => rectTransform.localPosition.sqrMagnitude)
            .First();
        return _pages.IndexOf(currentPage);
    }

    public int GetPositiveNearestPage()
    {
        var currentPage = _pages.Where(rectTransform => IsHorizontal? rectTransform.localPosition.x>0 : rectTransform.localPosition.y>0).OrderBy(rectTransform => rectTransform.localPosition.sqrMagnitude)
            .FirstOrDefault();
        return _pages.IndexOf(currentPage);
    }

    public int GetNegativeNearestPage()
    {
        var currentPage = _pages.Where(rectTransform =>IsHorizontal ? rectTransform.localPosition.x < 0 : rectTransform.localPosition.y < 0).OrderBy(rectTransform => rectTransform.localPosition.sqrMagnitude)
            .FirstOrDefault();
        return _pages.IndexOf(currentPage);
    }



    private void OnPageChanged()
    {
        if (_nextBtn!=null)
        {
            _nextBtn.interactable = CurrentPage < _pages.Count - 1;
        }

        if (_previousBtn != null)
        {
            _previousBtn.interactable = CurrentPage > 0;
        }
    }
}


//Animating
public partial class ViewPager
{
    [Header("Animating")]
    [SerializeField] private float _moveAnimTime = 0.7f;
    public bool MoveAnimating { get; private set; }
    private IEnumerator MoveToCurrentPage()
    {
        var targetPosition = -GetPositionForPage(CurrentPage);
        var lastPosition = PageContent.Position;
        var normalized = 0f;
        MoveAnimating = true;


        var normalizedTarget = 1.5f;
        //x+dx = x + (1.5 - x)*c*dt
        //dx = (1.5 -x)c*dt
        //dx/(1.5 - x) = cdt
        //-ln(1.5-x) + c1 = ct + c2
        var speed = -(Mathf.Log(normalizedTarget - 1) - Mathf.Log(normalizedTarget)) / _moveAnimTime;

        while (normalized<1)
        {
            
            
            normalized = Mathf.Clamp01(Mathf.Lerp(normalized, normalizedTarget, Time.deltaTime * speed));
            PageContent.Position = Vector2.Lerp(lastPosition,targetPosition,normalized);
            yield return null;
        }

        PageContent.Position = targetPosition;
        MoveAnimating = false;
    }

    private void StartMovingToCurrentPage()
    {
        StartCoroutine(MoveToCurrentPage());
    }

    private void StopAutoMovingIfMove()
    {
        if(!MoveAnimating)
            return;
        StopAllCoroutines();
        MoveAnimating = false;
    }
}

//Dragging
public partial class ViewPager:IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [Header("Drag")]
    [SerializeField] private bool _enableDrag = true;
    [SerializeField]private float _elasticDistance = 100;
    

    public bool EnableDrag
    {
        get { return _enableDrag; }
        set { _enableDrag = value; }
    }

    private bool _dragging;
    private readonly DragVelocityDetector _dragVelocityDetector = new DragVelocityDetector(5);

    public bool Dragging
    {
        get { return _dragging; }
        private set
        {
            _dragging = value;
            if(value)
                StopAutoMovingIfMove();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!EnableDrag)
            return;
        _dragVelocityDetector.OnStartDrag();
        Dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!EnableDrag)
            return;
        var inverseDelta = transform.InverseTransformVector(eventData.delta);
        _dragVelocityDetector.OnDrag(inverseDelta);
        var targetPosition = PageContent.Position + new Vector2(IsHorizontal? inverseDelta.x:0,IsHorizontal?0:inverseDelta.y);

        if (_moveDirection == Direction.LeftToRight &&(targetPosition.x > 0 || targetPosition.x < -PageContent.Length))
        {
            var currentDis = Mathf.Abs(targetPosition.x > 0 ? targetPosition.x : targetPosition.x + PageContent.Length);
            var velocity = Mathf.InverseLerp(_elasticDistance, 0, currentDis);
            PageContent.Position += (targetPosition - PageContent.Position) * velocity*velocity;
        }
        else if (_moveDirection == Direction.RightToLeft && (targetPosition.x < 0 || targetPosition.x > PageContent.Length))
        {
            var currentDis = Mathf.Abs(targetPosition.x < 0 ? targetPosition.x : targetPosition.x - PageContent.Length);
            var velocity = Mathf.InverseLerp(_elasticDistance, 0, currentDis);
            PageContent.Position += (targetPosition - PageContent.Position) * velocity * velocity;
        }
        else if (_moveDirection == Direction.TopToBottom && (targetPosition.y < 0 || targetPosition.y > PageContent.Length))
        {
            var currentDis = Mathf.Abs(targetPosition.y < 0 ? targetPosition.y : targetPosition.y - PageContent.Length);
            var velocity = Mathf.InverseLerp(_elasticDistance, 0, currentDis);
            PageContent.Position += (targetPosition - PageContent.Position) * velocity * velocity;
        }
        else if (_moveDirection == Direction.BottomToTop && (targetPosition.y > 0 || targetPosition.y < -PageContent.Length))
        {
            var currentDis = Mathf.Abs(targetPosition.y > 0 ? targetPosition.y : targetPosition.y + PageContent.Length);
            var velocity = Mathf.InverseLerp(_elasticDistance, 0, currentDis);
            PageContent.Position += (targetPosition - PageContent.Position) * velocity * velocity;
        }
        else
        {
            PageContent.Position = targetPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!EnableDrag)
        {
            return;
        }
        _dragVelocityDetector.OnStopDrag();
        Dragging = false;
        if (IsHorizontal && Mathf.Abs(_dragVelocityDetector.Velocity.x) > Screen.width * 1000f / 1080f)
        {
            CurrentPage = ((Vector2)_pages[GetNearestPage()].localPosition).sqrMagnitude >0 ?  
                (_dragVelocityDetector.Velocity.x > 0 ? GetNegativeNearestPage() : GetPositiveNearestPage()) : GetNearestPage();
            if (CurrentPage < 0)
            {
                Debug.Log("Invalid Current Page");
                CurrentPage = GetNearestPage();
            }
        }
        else if (!IsHorizontal && Mathf.Abs(_dragVelocityDetector.Velocity.y) > Screen.width*1000f/1080f)
        {
            CurrentPage = ((Vector2)_pages[GetNearestPage()].localPosition).sqrMagnitude >0 ?  
                (_dragVelocityDetector.Velocity.y > 0 ? GetNegativeNearestPage() : GetPositiveNearestPage()) : GetNearestPage();
            if (CurrentPage < 0)
            {
                Debug.Log("Invalid Current Page");
                CurrentPage = GetNearestPage();
            }
        }
        else
        {
            CurrentPage = GetNearestPage();
        }
        Debug.Log($"Drag Velocity:{_dragVelocityDetector.Velocity}");
        StartMovingToCurrentPage();
    }
}

//Other Class And Structs
public partial class ViewPager
{
    public enum Direction
    {
        LeftToRight,RightToLeft,TopToBottom,BottomToTop
    }

    public class Content
    {
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                var delta = value - _position;
                _pages.ForEach(rectTransform => rectTransform.localPosition += (Vector3) delta);
                _position = value;
            }
        }

        private readonly List<RectTransform> _pages = new List<RectTransform>();
        private Vector2 _position;
        public IEnumerable<RectTransform> Pages => _pages;

        public float Length =>
            _pages.Count == 0 ? 0 : (_pages.Last().localPosition - _pages.First().localPosition).magnitude;

        public Content(IEnumerable<RectTransform> pages, Vector2 position)
        {
            _pages.AddRange(pages);
            _position = position;
        }
    }
}

public partial class ViewPager
{
    private void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.T))
        {
            PageContent.Position = (-PageContent.Position.x < PageContent.Length / 2)
                ? new Vector2(-PageContent.Length, 0)
                : new Vector2(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            var currentPage = PageContent.Pages.OrderBy(rectTransform => rectTransform.localPosition.sqrMagnitude)
                .First();
            var i = _pages.IndexOf(currentPage);
            if (i < _pages.Count - 1)
            {
                SetPageIndex(i + 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            var currentPage = PageContent.Pages.OrderBy(rectTransform => rectTransform.localPosition.sqrMagnitude)
                .First();
            var i = _pages.IndexOf(currentPage);
            if (i > 0)
            {
                SetPageIndex(i - 1);
            }
        }
    }
}