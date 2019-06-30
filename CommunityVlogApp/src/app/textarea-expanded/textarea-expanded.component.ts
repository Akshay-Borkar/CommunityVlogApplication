import { Component, forwardRef, Renderer2, ViewChild } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

export const EPANDED_TEXTAREA_VALUE_ACCESSOR : any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => TextareaExpandedComponent),
  multi: true,
};

@Component({
  selector: 'textarea-expanded',
  providers: [EPANDED_TEXTAREA_VALUE_ACCESSOR],
  template: `
    <div contenteditable="true"
         #textarea
         class="input"
         tabindex="1"
         (input)="change($event)"
         role="textarea"></div>
  `,
  styles: [`
    div {
      width: 200px;
      min-height: 50px;
    }
    .input{
      border:1px solid #ccc;
      display:inline;
    }
    ul{
      border:1px solid red;
      display:inline-block;
      position:absolute;
    }
  `]
})
export class TextareaExpandedComponent implements ControlValueAccessor {
  @ViewChild('textarea') textarea;

  onChange;
  onTouched;

  writeValue( value : any ) : void {
    const div = this.textarea.nativeElement;
    this.renderer.setProperty(div, 'textContent', value);
  }

  registerOnChange( fn : any ) : void {
    this.onChange = fn;
  }

  registerOnTouched( fn : any ) : void {
    this.onTouched = fn;
  }

  // setDisabledState( isDisabled : boolean ) : void {
  //   const div = this.textarea.nativeElement;
  //   const action = isDisabled ? 'addClass' : 'removeClass';
  //   this.renderer[action](div, 'disabled');
  // }

  constructor( private renderer : Renderer2 ) {
  }

  change( $event ) {
    this.onChange($event.target.textContent);
    this.onTouched($event.target.textContent);
  }

}

// <textarea-expanded [formControl]="control"></textarea-expanded>