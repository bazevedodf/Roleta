import { AbstractControl, FormGroup } from "@angular/forms";

export class ValidatorField {
  public static ComparaCampos(primeiroCampo: string, segundoCampo: string): any {
    return (group: AbstractControl) => {
      const formGroup = group as FormGroup;
      const campo1 = formGroup.controls[primeiroCampo];
      const campo2 = formGroup.controls[segundoCampo];

      if (campo2.errors && !campo2.errors['invalido']) {
        return null;
      }

      if (campo1.value !== campo2.value) {
        campo2.setErrors({ invalido: true });
      } else {
        campo2.setErrors(null);
      }

      return null;
    }
  };

  public static ComparaSaldo(primeiroCampo: string, segundoCampo: string): any {
    return (group: AbstractControl) => {
      const formGroup = group as FormGroup;
      const valor = formGroup.controls[primeiroCampo];
      const saldo = formGroup.controls[segundoCampo];

      if (valor.errors && !valor.errors['withoutBalance']) {
        return null;
      }

      if (valor.value > saldo.value) {
        valor.setErrors({ withoutBalance: true });
      } else {
        valor.setErrors(null);
      }

      return null;
    }
  };
}
