(function () {
	var App = window.App || {};

	// ---------------------------------------------------------------------------
	// CPF utilities
	// ---------------------------------------------------------------------------
	App.cpf = (function () {
		function format(v) {
			v = (v || '').toString().replace(/\D/g, '').slice(0, 11);
			if (v.length > 9) return v.replace(/^(\d{3})(\d{3})(\d{3})(\d{0,2}).*/, '$1.$2.$3-$4');
			if (v.length > 6) return v.replace(/^(\d{3})(\d{3})(\d{0,3}).*/, '$1.$2.$3');
			if (v.length > 3) return v.replace(/^(\d{3})(\d{0,3}).*/, '$1.$2');
			return v;
		}
		function isValid(raw) {
			var v = (raw || '').replace(/\D/g, '');
			if (v.length !== 11) return false;
			if (/^(\d)\1{10}$/.test(v)) return false;
			function calc(len) {
				var sum = 0;
				for (var i = 0; i < len; i++) sum += parseInt(v.charAt(i), 10) * (len + 1 - i);
				var mod = sum % 11;
				return mod < 2 ? 0 : 11 - mod;
			}
			var d1 = calc(9);
			var d2 = calc(10);
			return d1 === parseInt(v.charAt(9), 10) && d2 === parseInt(v.charAt(10), 10);
		}
		function attachMask(el) {
			if (!el || el._cpfMaskAttached) return;
			el._cpfMaskAttached = true;
			var h = function () { el.value = format(el.value); };
			el.addEventListener('input', h);
			el.addEventListener('blur', h);
			h();
		}
		function attachValidation(el) {
			if (!el || el._cpfValidationAttached) return;
			el._cpfValidationAttached = true;
			var v = function () {
				var digits = (el.value || '').replace(/\D/g, '');
				if (digits.length === 11) {
					el.setCustomValidity(isValid(digits) ? '' : 'CPF inválido');
				} else {
					el.setCustomValidity('');
				}
			};
			el.addEventListener('input', v);
			el.addEventListener('blur', v);
			v();
		}
		function scan() {
			document.querySelectorAll('input.cpf-mask').forEach(attachMask);
			document.querySelectorAll('input.cpf-mask[data-validate-cpf="true"]').forEach(attachValidation);
		}
		return { scan: scan, isValid: isValid, format: format };
	})();

	// ---------------------------------------------------------------------------
	// Email utilities
	// ---------------------------------------------------------------------------
	App.email = (function () {
		function isValid(email) {
			if (!email) return false;
			email = email.trim();
			if (email.length > 100) return false;
			var parts = email.split('@');
			if (parts.length !== 2) return false;
			var local = parts[0], domain = parts[1];
			if (!local || !domain) return false;
			if (local.startsWith('.') || local.endsWith('.') || domain.startsWith('.') || domain.endsWith('.')) return false;
			if (local.indexOf('..') !== -1 || domain.indexOf('..') !== -1) return false;
			var localRe = /^[A-Za-z0-9.!#$%&'*+\/=?^_`{|}~-]+$/;
			if (!localRe.test(local)) return false;
			if (domain.indexOf('.') === -1) return false;
			var labels = domain.split('.');
			for (var i = 0; i < labels.length; i++) {
				var lab = labels[i];
				if (!/^[A-Za-z0-9-]{1,63}$/.test(lab)) return false;
				if (lab.startsWith('-') || lab.endsWith('-')) return false;
			}
			var tld = labels[labels.length - 1];
			if (!/^[A-Za-z]{2,63}$/.test(tld)) return false;
			return true;
		}
		function attach(el) {
			if (!el || el._emailValidationAttached) return;
			el._emailValidationAttached = true;
			var val = function () {
				var v = el.value || '';
				if (v.length === 0) { el.setCustomValidity(''); return; }
				el.setCustomValidity(isValid(v) ? '' : 'Email inválido');
			};
			el.addEventListener('input', val);
			el.addEventListener('blur', val);
			val();
		}
		function scan() {
			document.querySelectorAll('input[data-validate-email="true"]').forEach(attach);
		}
		return { scan: scan, isValid: isValid };
	})();

	// ---------------------------------------------------------------------------
	// Generic form helpers (masks, validation, create & details forms)
	// ---------------------------------------------------------------------------
	App.forms = (function () {
		function maskTel(v) {
			v = v.replace(/\D/g, '').slice(0, 11);
			if (v.length > 10) return '(' + v.slice(0, 2) + ') ' + v.slice(2, 7) + '-' + v.slice(7);
			if (v.length > 6) return '(' + v.slice(0, 2) + ') ' + v.slice(2, 6) + '-' + v.slice(6);
			if (v.length > 2) return '(' + v.slice(0, 2) + ') ' + v.slice(2);
			if (v.length > 0) return '(' + v;
			return '';
		}
		function maskDate(v) {
			v = v.replace(/\D/g, '').slice(0, 8);
			var r = '';
			if (v.length > 0) r += v.slice(0, 2);
			if (v.length >= 3) r += '/' + v.slice(2, 4);
			if (v.length >= 5) r += '/' + v.slice(4, 8);
			return r;
		}
		function attachBasicMasks(scope) {
			var tel = scope.querySelector('#Telefone');
			if (tel && !tel._telMask) {
				tel._telMask = true;
				tel.addEventListener('input', function () { if (this.disabled) return; this.value = maskTel(this.value); });
				tel.addEventListener('blur', function () { if (this.disabled) return; this.value = maskTel(this.value); });
				tel.value = maskTel(tel.value);
			}
			var dn = scope.querySelector('#DataNascimento');
			if (dn && !dn._dateMask) {
				dn._dateMask = true;
				dn.addEventListener('input', function () { if (this.disabled) return; this.value = maskDate(this.value); });
				dn.value = maskDate(dn.value);
			}
		}
		function initValidation() {
			var forms = document.querySelectorAll('.needs-validation');
			Array.prototype.slice.call(forms).forEach(function (form) {
				if (form._attachedValidation) return;
				form._attachedValidation = true;
				form.addEventListener('submit', function (e) {
					if (!form.checkValidity()) { e.preventDefault(); e.stopPropagation(); }
					form.classList.add('was-validated');
				}, false);
			});
		}
		function initCreate() {
			var form = document.getElementById('createForm');
			if (!form) return;
			attachBasicMasks(form);
			var papel = form.querySelector('#Papel');
			var cardAluno = document.getElementById('cardOutrasInfo');
			var cardProfessor = document.getElementById('cardProfessor');
			var matriculado = form.querySelector('#Matriculado');
			var motivoWrap = document.getElementById('wrapMotivoDesligamento');
			var motivo = form.querySelector('#MotivoDesligamento');
			var tipoProfessor = form.querySelector('#TipoProfessor');
			var repCPF = form.querySelector('#RepresentanteLegalCPF');
			var turma = form.querySelector('#Turma');
			function toggleMotivo() {
				if (matriculado && motivoWrap) {
					var mostrar = !matriculado.checked;
						motivoWrap.classList.toggle('d-none', !mostrar);
					if (motivo) {
						motivo.disabled = !mostrar;
						motivo.required = mostrar;
						if (mostrar) {
							motivo.setAttribute('minlength', '10');
							motivo.setAttribute('maxlength', '100');
						} else {
							motivo.removeAttribute('required');
							motivo.removeAttribute('minlength');
						}
					}
				}
			}
			function updateCards() {
				if (!papel) return;
				var isAluno = papel.value === 'Aluno';
				var isProfessor = papel.value === 'Professor';
				if (cardAluno) cardAluno.classList.toggle('d-none', !isAluno);
				if (cardProfessor) cardProfessor.classList.toggle('d-none', !isProfessor);
				if (isAluno && matriculado) { matriculado.checked = true; toggleMotivo(); }
				[repCPF, turma].forEach(function (el) {
					if (!el) return;
					if (isAluno) { el.required = true; el.disabled = false; } else { el.required = false; el.disabled = true; el.value = ''; }
				});
				if (tipoProfessor) {
					if (isProfessor) { tipoProfessor.disabled = false; tipoProfessor.required = true; }
					else { tipoProfessor.disabled = true; tipoProfessor.required = false; tipoProfessor.value = ''; }
				}
			}
			if (papel) {
				papel.addEventListener('change', function () { updateCards(); toggleMotivo(); });
				updateCards();
			}
			if (matriculado) { matriculado.addEventListener('change', toggleMotivo); toggleMotivo(); }
		}
		function initDetails() {
			var form = document.getElementById('detailsForm');
			if (!form) return;
			attachBasicMasks(form);
			var editarBtn = document.getElementById('btnEditar');
			var salvarBtn = document.getElementById('btnSalvar');
			var cancelarBtn = document.getElementById('btnCancelar');
			var voltarBtn = document.getElementById('btnVoltar');
			var papel = document.getElementById('Papel');
			var cpf = document.getElementById('CPF');
			var matriculado = document.getElementById('Matriculado');
			var wrapMotivo = document.getElementById('wrapMotivoDesligamento');
			var motivo = document.getElementById('MotivoDesligamento');
			var tipoProfessor = document.getElementById('TipoProfessor');
			var repCPF = document.getElementById('RepresentanteLegalCPF');
			var anoEscolar = document.getElementById('AnoEscolar');
			var isAluno = papel && papel.value === 'Aluno';
			var isProfessor = papel && papel.value === 'Professor';
			var initialValues = {};
			function snapshot() {
				Array.prototype.forEach.call(form.elements, function (el) {
					if (!el.name) return;
					initialValues[el.name] = el.type === 'checkbox' ? el.checked : el.value;
				});
			}
			snapshot();
			var editing = false;
			function toggleEdit(state) {
				editing = state;
				form.classList.toggle('editing', editing);
				var inputs = form.querySelectorAll('input,select');
				inputs.forEach(function (el) {
					if (el === cpf || el === papel) return;
					if (editing) { el.removeAttribute('disabled'); } else { el.setAttribute('disabled', 'disabled'); }
				});
				if (cpf) cpf.setAttribute('disabled', 'disabled');
				if (papel) papel.setAttribute('disabled', 'disabled');
				if (editarBtn) editarBtn.classList.toggle('d-none', editing);
				if (salvarBtn) salvarBtn.classList.toggle('d-none', !editing);
				if (cancelarBtn) cancelarBtn.classList.toggle('d-none', !editing);
				if (voltarBtn) voltarBtn.classList.toggle('d-none', editing);
				applyRoleLogic();
			}
			function applyRoleLogic() {
				if (editing) {
					if (isAluno) {
						[repCPF, document.getElementById('Turma')].forEach(function (el) {
							if (!el) return;
							el.required = true;
							el.disabled = false;
						});
						if (matriculado) matriculado.disabled = false;
						if (tipoProfessor) { tipoProfessor.required = false; tipoProfessor.disabled = true; }
						if (matriculado && wrapMotivo && motivo) {
							var mostrar = !matriculado.checked;
							wrapMotivo.classList.toggle('d-none', !mostrar);
							motivo.disabled = !mostrar;
							motivo.required = mostrar;
							if (mostrar) {
								motivo.setAttribute('minlength', '10');
								motivo.setAttribute('maxlength', '100');
							} else {
								motivo.removeAttribute('required');
								motivo.removeAttribute('minlength');
							}
						}
					} else if (isProfessor) {
						if (tipoProfessor) { tipoProfessor.required = true; tipoProfessor.disabled = false; }
						[repCPF, document.getElementById('Turma'), matriculado, anoEscolar].forEach(function (el) {
							if (!el) return;
							el.disabled = true;
							el.required = false;
						});
						if (wrapMotivo && motivo) {
							wrapMotivo.classList.add('d-none');
							motivo.disabled = true;
							motivo.required = false;
						}
					}
				} else {
					[repCPF, document.getElementById('Turma'), tipoProfessor, matriculado, anoEscolar, motivo].forEach(function (el) {
						if (!el) return;
						el.disabled = true;
						el.required = false;
					});
					if (wrapMotivo) wrapMotivo.classList.add('d-none');
				}
			}
			if (editarBtn) editarBtn.addEventListener('click', function () { toggleEdit(true); });
			if (cancelarBtn) cancelarBtn.addEventListener('click', function () {
				Array.prototype.forEach.call(form.elements, function (el) {
					if (!el.name) return;
					if (initialValues.hasOwnProperty(el.name)) {
						if (el.type === 'checkbox') { el.checked = initialValues[el.name]; }
						else { el.value = initialValues[el.name]; }
					}
				});
				toggleEdit(false);
			});
			if (salvarBtn) salvarBtn.addEventListener('click', function (e) {
				if (!form.checkValidity()) {
					e.preventDefault();
					e.stopPropagation();
					form.classList.add('was-validated');
					return;
				}
				toggleEdit(false); // placeholder: would submit via fetch/ajax
			});
			if (matriculado) matriculado.addEventListener('change', function () { applyRoleLogic(); });
			toggleEdit(false);
		}
		function init() {
			App.cpf.scan();
			App.email.scan();
			initValidation();
			initCreate();
			initDetails();
		}
		return { init: init };
	})();

	window.App = App;
	function boot() { App.forms.init(); }
	if (document.readyState === 'loading') { document.addEventListener('DOMContentLoaded', boot); } else { boot(); }
})();
