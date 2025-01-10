import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FullUrlComponent } from './full-url.component';

describe('FullUrlComponent', () => {
  let component: FullUrlComponent;
  let fixture: ComponentFixture<FullUrlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FullUrlComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FullUrlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
