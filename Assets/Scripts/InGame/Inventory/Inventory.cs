using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    [SerializeField]
    private InventoryUI _inventoryUI;
    [SerializeField]
    private Item[] _items;
    
    [SerializeField]
    private EquipUI _equipmentUI;
    [SerializeField]
    private Item[] _equ;

    [SerializeField]
    private ActiveSlotUI _eSlot;

    [SerializeField]
    private ActiveSlotUI _iSlot;

    public PlayerStat InvenStat;
    private int _maxCapacity = 80;
    private int _equCount = 5;

    public int USESTARTINDEX = 90;
    public int _useSlotindex;

    



    private void Awake()
    {
        
        _items = new Item[_maxCapacity];
        _equ = new Item[_equCount];
        Capacity = _maxCapacity;
        ECapacity = _equCount;
        _equipmentUI.SetInventoryReference(this);
        _inventoryUI.SetInventoryReference(this);
        _useSlotindex = USESTARTINDEX;
    }

    public int Capacity { get; private set; }
    public int ECapacity { get; private set; }

    public int Add(ItemData itemData, int amount = 1)
    {
        int index;

        // 1. 수량이 있는 아이템
        if (itemData is CountableItemData ciData)
        {
            bool findNextCountable = true;
            index = -1;
            while (amount > 0)
            {
                // 1-1. 이미 해당 아이템이 인벤토리 내에 존재하고, 개수 여유 있는지 검사
                if (findNextCountable)
                {
                    index = FindCountableItemSlotIndex(ciData, index + 1);

                    // 개수 여유있는 기존재 슬롯이 더이상 없다고 판단될 경우, 빈 슬롯부터 탐색 시작
                    if (index == -1)
                    {
                        findNextCountable = false;
                    }
                    // 기존재 슬롯을 찾은 경우, 양 증가시키고 초과량 존재 시 amount에 초기화
                    else
                    {
                        CountableItem ci = _items[index] as CountableItem;
                        amount = ci.AddAmountAndGetExcess(amount);

                        UpdateSlot(index);
                    }
                }
                // 1-2. 빈 슬롯 탐색
                else
                {
                    index = FindEmptySlotIndex(index + 1);

                    // 빈 슬롯조차 없는 경우 종료
                    if (index == -1)
                    {
                        break;
                    }
                    // 빈 슬롯 발견 시, 슬롯에 아이템 추가 및 잉여량 계산
                    else
                    {
                        // 새로운 아이템 생성
                        CountableItem ci = ciData.CreateItem() as CountableItem;
                        ci.SetAmount(amount);

                        // 슬롯에 추가
                        _items[index] = ci;

                        // 남은 개수 계산
                        amount = (amount > ciData.MaxAmount) ? (amount - ciData.MaxAmount) : 0;

                        UpdateSlot(index);
                    }
                }
            }
        }
        // 2. 수량이 없는 아이템
        else
        {
            // 2-1. 1개만 넣는 경우, 간단히 수행
            if (amount == 1)
            {
                index = FindEmptySlotIndex();
                if (index != -1)
                {
                    // 아이템을 생성하여 슬롯에 추가
                    _items[index] = itemData.CreateItem();
                    amount = 0;

                    UpdateSlot(index);
                }
                else
                {
                    Debug.Log("err");
                }
            }

            // 2-2. 2개 이상의 수량 없는 아이템을 동시에 추가하는 경우
            index = -1;
            for (; amount > 0; amount--)
            {
                // 아이템 넣은 인덱스의 다음 인덱스부터 슬롯 탐색
                index = FindEmptySlotIndex(index + 1);

                // 다 넣지 못한 경우 루프 종료
                if (index == -1)
                {
                    break;
                }

                // 아이템을 생성하여 슬롯에 추가
                _items[index] = itemData.CreateItem();

                UpdateSlot(index);
            }
        }

        return amount;
    }
    public void ShowActiveESlot(int index)
    {
        if(_equ[index] is EquipmentItem _uItem)
        {
            _eSlot.SetStat(_uItem.EquipmentData.Stat);
        }
    }
    public void ShowActiveSlot(int index)
    {

    }
    public void Use(int index)
    {
        if (!IsValidIndex(index)) return;
        if (_items[index] == null) return;

        // 사용 가능한 아이템인 경우
        if (_items[index] is IUsableItem uItem)
        {
            // 아이템 사용
            bool succeeded = uItem.Use();

            if (succeeded)
            {
                UpdateSlot(index);
            }
        }
        else if (_items[index] is EquipmentItem _uItem)
        {
            
            Equip(_items[index]);
            Remove(index);
        }
    }

    public void UnEquip(int index)
    {
        if (!IsValidIndex(index)) return;
        if (_equ[index] == null) return;

        if (_equ[index] is EquipmentItem _uItem)
        {
            Add(_equ[index].Data);
            EquRemove(index);
           
        }
    }
    public void Equip(Item _item)
    {
        int _slotNum;
        if (_item is EquipmentItem _uItem)
        {
            _slotNum = _uItem.EquipmentData.EquType;
            if (!CheckEquSlot(_slotNum))
            {
                UnEquip(_slotNum);
            }
            _equ[_slotNum] = _item;
            UpdateEqu(_slotNum);

            Debug.Log(_uItem.EquipmentData.EquType);
        }
    }
    public bool CheckEquSlot(int _slotNum)
    {
        Item item = _equ[_slotNum];

        return item == null;
    }
    public void UpdateEqu(int index)
    {
        if (!IsValidIndex(index)) return;

        Item item = _equ[index];

        // 1. 아이템이 슬롯에 존재하는 경우
        if (item != null)
        {
            // 아이콘 등록
            _equipmentUI.SetItemIcon(index, item.Data.IconSprite);
        }
        // 2. 빈 슬롯인 경우 : 아이콘 제거
        else
        {
            RemoveIcon();
        }

        // 로컬 : 아이콘 제거하기
        void RemoveIcon()
        {
            _equipmentUI.RemoveItem(index);
        }
    }
    /// <summary> 해당하는 인덱스의 슬롯 상태 및 UI 갱신 </summary>
    private void UpdateSlot(int index)
    {
        if (!IsValidIndex(index)) return;

        Item item = _items[index];

        // 1. 아이템이 슬롯에 존재하는 경우
        if (item != null)
        {
            // 아이콘 등록
           
            _inventoryUI.SetItemIcon(index, item.Data.IconSprite);

            // 1-1. 셀 수 있는 아이템
            if (item is CountableItem ci)
            {
                // 1-1-1. 수량이 0인 경우, 아이템 제거
                if (ci.IsEmpty)
                {
                    _items[index] = null;
                    RemoveIcon();
                    return;
                }
                // 1-1-2. 수량 텍스트 표시
                else
                {
                    _inventoryUI.SetItemAmountText(index, ci.Amount);
                }
            }
            // 1-2. 셀 수 없는 아이템인 경우 수량 텍스트 제거
            else
            {
                _inventoryUI.HideItemAmountText(index);
            }

            // 슬롯 필터 상태 업데이트
            //_inventoryUI.UpdateSlotFilterState(index, item.Data);
        }
        // 2. 빈 슬롯인 경우 : 아이콘 제거
        else
        {
            RemoveIcon();
        }

        // 로컬 : 아이콘 제거하기
        void RemoveIcon()
        {
            _inventoryUI.RemoveItem(index);
            _inventoryUI.HideItemAmountText(index); // 수량 텍스트 숨기기
        }
    }
    /// <summary> 앞에서부터 개수 여유가 있는 Countable 아이템의 슬롯 인덱스 탐색 </summary>
    private int FindCountableItemSlotIndex(CountableItemData target, int startIndex = 0)
    {
        for (int i = startIndex; i < Capacity; i++)
        {
            var current = _items[i];
            if (current == null)
                continue;

            // 아이템 종류 일치, 개수 여유 확인
            if (current.Data == target && current is CountableItem ci)
            {
                if (!ci.IsMax)
                    return i;
            }
        }

        return -1;
    }
    private int FindEmptySlotIndex(int startIndex = 0)
    {
        for (int i = startIndex; i < Capacity; i++)
            if (_items[i] == null)
                return i;
        return -1;
    }
    /// <summary> 인덱스가 수용 범위 내에 있는지 검사 </summary>
    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < Capacity;
    }
    /// <summary> 해당 슬롯의 아이템 제거 </summary>
    public void Remove(int index)
    {
        if (!IsValidIndex(index)) return;

        _items[index] = null;
        _inventoryUI.RemoveItem(index);
    }
    public void EquRemove(int index)
    {
        if (!IsValidIndex(index)) return;

        _equ[index] = null;
        _equipmentUI.RemoveItem(index);
    }

}