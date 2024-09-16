pip install numpy scipy matplotlib
import numpy as np
import matplotlib.pyplot as plt
from scipy import signal

# Définir la fréquence de coupure du pôle
fc = 1e3  # Fréquence de coupure en Hz (par exemple 1 kHz)
w_c = 2 * np.pi * fc

# Créer un système avec un pôle à la fréquence de coupure
numerator = [1]
denominator = [1, w_c]  # Fonction de transfert 1 / (s + w_c)
system = signal.TransferFunction(numerator, denominator)

# Générer des fréquences pour l'analyse
frequencies = np.logspace(1, 5, num=100)  # De 10 Hz à 100 kHz
w = 2 * np.pi * frequencies

# Calculer la réponse du système
w, mag, phase = signal.bode(system, w=w)

# Tracer les graphiques de Bode
fig, (ax1, ax2) = plt.subplots(2, 1, figsize=(10, 8))

# Graphique de Gain
ax1.semilogx(frequencies, mag)
ax1.set_title('Diagramme de Bode')
ax1.set_xlabel('Fréquence [Hz]')
ax1.set_ylabel('Gain [dB]')
ax1.grid(which='both', axis='both')

# Graphique de Phase
ax2.semilogx(frequencies, phase)
ax2.set_xlabel('Fréquence [Hz]')
ax2.set_ylabel('Phase [degrés]')
ax2.grid(which='both', axis='both')

plt.tight_layout()
plt.show()

