import { MatDialogRef } from '@angular/material/dialog';
import { AppPopupResult } from '../types';

export abstract class BasePopup<TComponent>
{
    protected constructor(
        protected readonly dialogRef: MatDialogRef<TComponent>
    ) {
    }

    cancel()
    {
        const result: AppPopupResult = {isDirty: false};
        this.dialogRef.close(result);
    }

    markToClose(data?: any)
    {
        const result: AppPopupResult = {isDirty: true, data};
        this.dialogRef.close(result);
    }
}
