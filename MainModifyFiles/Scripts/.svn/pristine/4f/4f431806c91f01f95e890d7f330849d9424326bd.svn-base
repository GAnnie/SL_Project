using UnityEngine;
using System.Collections;

public class PetChangeNameViewController : MonoBehaviour,IViewController {

	private PetChangeNameView _view;
	private PetPropertyInfo _petInfo;

	public void Open(PetPropertyInfo petInfo){
		_petInfo = petInfo;
		InitView();
	}

	#region IViewController implementation
	public void InitView ()
	{
		_view = gameObject.GetMissingComponent<PetChangeNameView>();
		_view.Setup(this.transform);
		_view.nameInput.characterLimit = 5;

		RegisterEvent();
	}
	public void RegisterEvent ()
	{
		EventDelegate.Set(_view.defualtBtn.onClick,OnClickDefaultBtn);
		EventDelegate.Set(_view.confirmBtn.onClick,OnClickConfirmBtn);
		EventDelegate.Set(_view.cancelBtn.onClick,OnClickCancelBtn);
	}
	public void Dispose ()
	{
	}
	#endregion

	private void OnClickDefaultBtn(){
		_view.nameInput.value = _petInfo.pet.name;
	}

	private void OnClickConfirmBtn(){
		if(string.IsNullOrEmpty(_view.nameInput.value))
			TipManager.AddTip("宠物名字不能为空");
		else
			PetModel.Instance.ChangePetName(_petInfo.petDto.id,_view.nameInput.value);
	}

	private void OnClickCancelBtn(){
		ProxyPetPropertyModule.CloseChangeNameView();
	}
}
