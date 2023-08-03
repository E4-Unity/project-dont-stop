using System;
using System.Collections.Generic;

public interface ICheckInit
{
    // 해당 스크립트가 초기화가 완료되었는지 확인
    // Set 호출 금지. Extension 전용
    public bool IsInitialized { get; set; }
    
    // 초기화가 되지 않은 경우, 초기화가 완료되면 호출할 Action 목록
    public List<Action> OnInitActions { get; }
}

public static class CheckInitExtension
{
    public static void TryInit(this ICheckInit _checkInit, Action _initAction)
    {
        if (_checkInit.IsInitialized)
            _initAction();
        else
            _checkInit.OnInitActions.Add(_initAction);
    }

    public static void FinishInit(this ICheckInit _checkInit)
    {
        _checkInit.IsInitialized = true;
        foreach (var action in _checkInit.OnInitActions)
        {
            action();
        }
        
        _checkInit.OnInitActions.Clear();
    }
}