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
    private ActiveSlotUI _activeESlot;

    [SerializeField]
    private ActiveSlotUI _activeISlot;

    public Stat InvenStat;
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

        // 1. ������ �ִ� ������
        if (itemData is CountableItemData ciData)
        {
            bool findNextCountable = true;
            index = -1;
            while (amount > 0)
            {
                // 1-1. �̹� �ش� �������� �κ��丮 ���� �����ϰ�, ���� ���� �ִ��� �˻�
                if (findNextCountable)
                {
                    index = FindCountableItemSlotIndex(ciData, index + 1);

                    // ���� �����ִ� ������ ������ ���̻� ���ٰ� �Ǵܵ� ���, �� ���Ժ��� Ž�� ����
                    if (index == -1)
                    {
                        findNextCountable = false;
                    }
                    // ������ ������ ã�� ���, �� ������Ű�� �ʰ��� ���� �� amount�� �ʱ�ȭ
                    else
                    {
                        CountableItem ci = _items[index] as CountableItem;
                        amount = ci.AddAmountAndGetExcess(amount);

                        UpdateSlot(index);
                    }
                }
                // 1-2. �� ���� Ž��
                else
                {
                    index = FindEmptySlotIndex(index + 1);

                    // �� �������� ���� ��� ����
                    if (index == -1)
                    {
                        break;
                    }
                    // �� ���� �߰� ��, ���Կ� ������ �߰� �� �׿��� ���
                    else
                    {
                        // ���ο� ������ ����
                        CountableItem ci = ciData.CreateItem() as CountableItem;
                        ci.SetAmount(amount);

                        // ���Կ� �߰�
                        _items[index] = ci;

                        // ���� ���� ���
                        amount = (amount > ciData.MaxAmount) ? (amount - ciData.MaxAmount) : 0;

                        UpdateSlot(index);
                    }
                }
            }
        }
        // 2. ������ ���� ������
        else
        {
            // 2-1. 1���� �ִ� ���, ������ ����
            if (amount == 1)
            {
                index = FindEmptySlotIndex();
                if (index != -1)
                {
                    // �������� �����Ͽ� ���Կ� �߰�
                    _items[index] = itemData.CreateItem();
                    amount = 0;

                    UpdateSlot(index);
                }
                else
                {
                    Debug.Log("err");
                }
            }

            // 2-2. 2�� �̻��� ���� ���� �������� ���ÿ� �߰��ϴ� ���
            index = -1;
            for (; amount > 0; amount--)
            {
                // ������ ���� �ε����� ���� �ε������� ���� Ž��
                index = FindEmptySlotIndex(index + 1);

                // �� ���� ���� ��� ���� ����
                if (index == -1)
                {
                    break;
                }

                // �������� �����Ͽ� ���Կ� �߰�
                _items[index] = itemData.CreateItem();

                UpdateSlot(index);
            }
        }

        return amount;
    }
    public void ShowActiveESlot(int index)
    {
        _activeESlot.UpdateUI(_equ[index]);
    }
    public void ShowActiveISlot(int index)
    {
        _activeISlot.UpdateUI(_items[index]);
    }
    public void HideActiveESlot()
    {
        _activeESlot.ResetUI();
    }
    public void HideActiveISlot()
    {
        _activeISlot.ResetUI();
    }
    public void Use(int index)
    {
        if (!IsValidIndex(index)) return;
        if (_items[index] == null) return;

        // ��� ������ �������� ���
        if (_items[index] is IUsableItem uItem)
        {
            // ������ ���
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
            UnEquPlayerStat(_uItem.EquipmentData.Stat);
            EquRemove(index);
        }
        
    }
    public void EquPlayerStat(ItemStat _stat)
    {
        
        DataManager.Instance.Player.Stat.Attack += _stat.Attack;
        DataManager.Instance.Player.Stat.HitPercent += _stat.HitPercent;
        DataManager.Instance.Player.Stat.SkillDamage += _stat.SkillDamage;
        DataManager.Instance.Player.Stat.AllDamge += _stat.AllDamge;

        DataManager.Instance.Player.Stat.Defense += _stat.Defense;
        DataManager.Instance.Player.Stat.Dodge += _stat.Dodge;
        DataManager.Instance.Player.Stat.ReduceDamage += _stat.ReduceDamage;

        DataManager.Instance.Player.Stat.MaxHp += _stat.MaxHp;
        DataManager.Instance.Player.Stat.MaxMp += _stat.MaxMp;
     //   DataManager.Instance.Player.Stat.+= _stat.Attack;
        DataManager.Instance.Player.Stat.RecoveryHp += _stat.RecoveryHp;
     //   DataManager.Instance.Player.Stat.re += _stat.Attack;
        
    }
    public void UnEquPlayerStat(ItemStat _stat)
    {

        DataManager.Instance.Player.Stat.Attack -= _stat.Attack;
        DataManager.Instance.Player.Stat.HitPercent -= _stat.HitPercent;
        DataManager.Instance.Player.Stat.SkillDamage -= _stat.SkillDamage;
        DataManager.Instance.Player.Stat.AllDamge -= _stat.AllDamge;

        DataManager.Instance.Player.Stat.Defense -= _stat.Defense;
        DataManager.Instance.Player.Stat.Dodge -= _stat.Dodge;
        DataManager.Instance.Player.Stat.ReduceDamage -= _stat.ReduceDamage;

        DataManager.Instance.Player.Stat.MaxHp -= _stat.MaxHp;
        DataManager.Instance.Player.Stat.MaxMp -= _stat.MaxMp;
        //   DataManager.Instance.Player.Stat.+= _stat.Attack;
        DataManager.Instance.Player.Stat.RecoveryHp += _stat.RecoveryHp;
        //   DataManager.Instance.Player.Stat.re += _stat.Attack;

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
            EquPlayerStat(_uItem.EquipmentData.Stat);
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

        // 1. �������� ���Կ� �����ϴ� ���
        if (item != null)
        {
            // ������ ���
            _equipmentUI.SetItemIcon(index, item.Data.IconSprite);
        }
        // 2. �� ������ ��� : ������ ����
        else
        {
            RemoveIcon();
        }

        // ���� : ������ �����ϱ�
        void RemoveIcon()
        {
            _equipmentUI.RemoveItem(index);
        }
    }
    /// <summary> �ش��ϴ� �ε����� ���� ���� �� UI ���� </summary>
    private void UpdateSlot(int index)
    {
        if (!IsValidIndex(index)) return;

        Item item = _items[index];

        // 1. �������� ���Կ� �����ϴ� ���
        if (item != null)
        {
            // ������ ���
           
            _inventoryUI.SetItemIcon(index, item.Data.IconSprite);

            // 1-1. �� �� �ִ� ������
            if (item is CountableItem ci)
            {
                // 1-1-1. ������ 0�� ���, ������ ����
                if (ci.IsEmpty)
                {
                    _items[index] = null;
                    RemoveIcon();
                    return;
                }
                // 1-1-2. ���� �ؽ�Ʈ ǥ��
                else
                {
                    _inventoryUI.SetItemAmountText(index, ci.Amount);
                }
            }
            // 1-2. �� �� ���� �������� ��� ���� �ؽ�Ʈ ����
            else
            {
                _inventoryUI.HideItemAmountText(index);
            }

            // ���� ���� ���� ������Ʈ
            //_inventoryUI.UpdateSlotFilterState(index, item.Data);
        }
        // 2. �� ������ ��� : ������ ����
        else
        {
            RemoveIcon();
        }

        // ���� : ������ �����ϱ�
        void RemoveIcon()
        {
            _inventoryUI.RemoveItem(index);
            _inventoryUI.HideItemAmountText(index); // ���� �ؽ�Ʈ �����
        }
    }
    /// <summary> �տ������� ���� ������ �ִ� Countable �������� ���� �ε��� Ž�� </summary>
    private int FindCountableItemSlotIndex(CountableItemData target, int startIndex = 0)
    {
        for (int i = startIndex; i < Capacity; i++)
        {
            var current = _items[i];
            if (current == null)
                continue;

            // ������ ���� ��ġ, ���� ���� Ȯ��
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
    /// <summary> �ε����� ���� ���� ���� �ִ��� �˻� </summary>
    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < Capacity;
    }
    /// <summary> �ش� ������ ������ ���� </summary>
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